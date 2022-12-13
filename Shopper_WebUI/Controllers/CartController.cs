﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shopper_BLL.Abstract;
using Shopper_WebUI.Identity;
using Shopper_WebUI.Models;
using System.Net;
using System;
using IyzipayCore.Request;
using IyzipayCore.Model;
using Options = IyzipayCore.Options;
using Shopper_Entity;

namespace Shopper_WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<ApplicationUser> _userManager;
        private IOrderService _orderService;

        public CartController(ICartService cartService,UserManager<ApplicationUser> userManager,IOrderService orderService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            return View(new CartModel()
            {
                CartId=cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId=i.Id,
                    ProductId=i.Product.Id,
                    Name=i.Product.Name,
                    Price=i.Product.Price,
                    ImageUrl = i.Product.Images[0].ImageUrl,
                    Quantity=i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            _cartService.AddToCart(_userManager.GetUserId(User), productId, quantity);
            return RedirectToAction("Index");
        }

		[HttpPost]
		public IActionResult DeleteFromCart(int productId)
		{
            _cartService.DeleteFromCart(_userManager.GetUserId(User), productId);
			return RedirectToAction("Index");
		}

        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId=cart.Id,
                CartItems=cart.CartItems.Select(i=> new CartItemModel()
                {
                    CartItemId=i.Id,
                    ProductId=i.Product.Id,
                    Name=i.Product.Name,
                    Price=i.Product.Price,
                    ImageUrl = i.Product.Images[0].ImageUrl,
                    Quantity=i.Quantity
                }).ToList()
            };


            return View(orderModel);
        }

        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            ModelState.Remove("CartModel");
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.Product.Id,
                        Name = i.Product.Name,
                        Price = (decimal)i.Product.Price,
                        ImageUrl = i.Product.Images[0].ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };

                //Ödeme
                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(cart.Id.ToString());

                    return View("Success");
                }

                //Sipariş
            }

            return View(model);
        }

        private void ClearCart(string cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.Completed;
            order.PaymentTypes = EnumPaymentTypes.CreditCart;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.OrderNote = "Zil Çalma Bebek Uyuyor.";
            order.City = model.City;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.Address = model.Address;
            order.UserId = userId;

            foreach (var item in model.CartModel.CartItems)
            {
                var orderitem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };

                order.OrderItems.Add(orderitem);
            }

            _orderService.Create(order);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-A0FJYvo7hAleF1cAwNSYodVjELAj6EsB";
            options.SecretKey = "sandbox-OtWNn0GhKEKH0SVuwQcKYv1ZpEfDXCtD";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            
            #region Payment
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0];
            request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0];
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = model.CartModel.CartId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth =model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = model.Phone;
            buyer.Email = model.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = model.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;
            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = (item.Price * item.Quantity).ToString().Split(",")[0];
                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;


            return Payment.Create(request, options);
            #endregion

        }
    }
}
