using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using Project.Models.Models;
using Project.Models.ViewModels;
using Project.Utility;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Diagnostics;
using System.Security.Claims;

namespace TheReadHaven.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
	
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public OrderVM OrderVM { get; set; }
		public OrderController(IUnitOfWork unitOfWork)
		{
		_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			return View();

		}

	
		public IActionResult Details(int orderId)
		{
			OrderVM orderVM = new()
			{
				OrderHeader =_unitOfWork.OrderHeader.Get(u=>u.Id==orderId,includeProperties:"ApplicationUser"),
			     OrderDetail = _unitOfWork.OrderDetail.GetAll(u=>u.OrderHeaderId==orderId,includeProperties:"Product")
			};
			return View(orderVM);
		}

		[HttpPost]
		[Authorize(Roles =StaticDetails.Role_Admin+","+StaticDetails.Role_Employee)]
        public IActionResult UpdateOrderDetails()
        {

			var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
			orderHeaderFromDb.Name= OrderVM.OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber= OrderVM.OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAddress= OrderVM.OrderHeader.StreetAddress;
			orderHeaderFromDb.City= OrderVM.OrderHeader.City;
			orderHeaderFromDb.State= OrderVM.OrderHeader.State;
			orderHeaderFromDb.PostalCode= OrderVM.OrderHeader.PostalCode;
			if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
			{
				orderHeaderFromDb.Carrier=OrderVM.OrderHeader.Carrier;
			}
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TRackingNumber))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TRackingNumber;
            }
			_unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();
			TempData["Success"] = "Order Details Updated Successfully";

            return RedirectToAction(nameof(Details),new {orderId=orderHeaderFromDb.Id});
        }

		[HttpPost]
		[Authorize(Roles =StaticDetails.Role_Admin+","+StaticDetails.Role_Employee)]
		public IActionResult StartProcessing()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, StaticDetails.StatusInProcess);
			_unitOfWork.Save();
			TempData["Success"] = "Order details updated Successfully";
			return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
		}

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        public IActionResult ShipOrder()
        {
			var orderHeader=_unitOfWork.OrderHeader.Get(u=>u.Id==OrderVM.OrderHeader.Id);
			orderHeader.TRackingNumber = OrderVM.OrderHeader.TRackingNumber;
			orderHeader.Carrier=OrderVM.OrderHeader.Carrier;
			orderHeader.OrderStatus = StaticDetails.StatusShipped;
			orderHeader.ShippingDate=DateTime.Now;
			if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
			{
				orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
			}
			_unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

		[HttpPost]
		[Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
		public IActionResult CancelOrder()
		{
			var orderHeader=_unitOfWork.OrderHeader.Get(u=>u.Id== OrderVM.OrderHeader.Id);
			if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusApproved)
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderHeader.PaymentIntentId
				};
				var service = new RefundService();
				Refund refund = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, StaticDetails.StatusCancelled, StaticDetails.StatusRefunded);
			}
			else
			{
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, StaticDetails.StatusCancelled, StaticDetails.StatusCancelled);

            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
       }

		[ActionName("Details")]
		[HttpPost]
		public IActionResult Details_Pay_Now()
		{
			OrderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
			OrderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == OrderVM.OrderHeader.Id, includeProperties: "Product");
            //stripe logic
            //capture payment of regular customer
            var domain = "https://localhost:7208/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
				SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in OrderVM.OrderDetail)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderHeaderId);
            if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
            {
				//this is order by company
                var service = new Stripe.Checkout.SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, StaticDetails.StatusApproved);
                    _unitOfWork.Save();
                }
            }
              return View(orderHeaderId);
        }

        #region API CALLS

        [HttpGet]
		public IActionResult GetAll(string status) {

		IEnumerable<OrderHeader> objOrderHeaders =_unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser").ToList();

			if(User.IsInRole(StaticDetails.Role_Admin)||User.IsInRole(StaticDetails.Role_Employee))
			{
				objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
			}
			else
			{
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==userId, includeProperties: "ApplicationUser").ToList();

            }

            switch (status)
            {
                case "pending":
					objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
					break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }


            return Json(new { Data = objOrderHeaders });
		}
		#endregion
	}
}
