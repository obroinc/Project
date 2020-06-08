using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Models.ViewModels;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace ClubWestRFC.Pages.Admin.Order
{
    public class ManageOrderModel : PageModel
    {
        
            private readonly IUnitofWork _unitofWork;

            public ManageOrderModel(IUnitofWork unitOfWork)
            {
                _unitofWork = unitOfWork;
            }

            [BindProperty]
            public List<OrderDetailsViewModel> orderDetailsVM { get; set; }


            public void OnGet()
            {
                orderDetailsVM = new List<OrderDetailsViewModel>();

                List<OrderHeader> OrderHeaderList = _unitofWork.OrderHeader
                    .GetAll(o => o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                    .OrderByDescending(u => u.PickUpTime).ToList();


                foreach (OrderHeader item in OrderHeaderList)
                {
                    OrderDetailsViewModel individual = new OrderDetailsViewModel
                    {
                        OrderHeader = item,
                        OrderDetails = _unitofWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                    };
                    orderDetailsVM.Add(individual);
                }
            }

            public IActionResult OnPostOrderPrepare(int orderId)
            {
                OrderHeader orderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
                orderHeader.Status = SD.StatusInProcess;
                _unitofWork.Save();
                return RedirectToPage("ManageOrder");
            }

            public IActionResult OnPostOrderReady(int orderId)
            {
                OrderHeader orderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
                orderHeader.Status = SD.StatusReady;
                _unitofWork.Save();
                return RedirectToPage("ManageOrder");
            }

            public IActionResult OnPostOrderCancel(int orderId)
            {
                OrderHeader orderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
                orderHeader.Status = SD.StatusCancelled;
                _unitofWork.Save();
                return RedirectToPage("ManageOrder");
            }

            public IActionResult OnPostOrderRefund(int orderId)
            {
                OrderHeader orderHeader = _unitofWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
                //refund amount
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                   Charge = orderHeader.TransactionId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.Status = SD.StatusRefunded;
                _unitofWork.Save();
                return RedirectToPage("ManageOrder");
            }

    }
}
