﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CheckoutNetsdk.Core;
using CheckoutNetsdk.Orders;
using BraintreeHttp;

namespace Samples.CaptureIntentExamples
{
    public class CreateOrderSample
    {
       
        /*
            Method to generate sample create order body with <b>CAPTURE</b> intent
            
            @return OrderRequest with created order request
         */
        private static OrderRequest BuildRequestBody()
        {
            OrderRequest orderRequest = new OrderRequest()
            {
                Intent = "CAPTURE",

                ApplicationContext = new ApplicationContext
                {
                    BrandName = "EXAMPLE INC",
                    LandingPage = "BILLING",
                    CancelUrl = "https://www.google.com",
                    ReturnUrl = "https://www.google.com",
                    UserAction = "CONTINUE",
                    ShippingPreference = "SET_PROVIDED_ADDRESS"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest{
                        ReferenceId =  "PUHF",
                        Description = "Sporting Goods",
                        CustomId = "CUST-HighFashions",
                        SoftDescriptor = "HighFashions",
                        Amount = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = "230.00",
                            Breakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "180.00"
                                },
                                Shipping = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "30.00"
                                },
                                Handling = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "10.00"
                                },
                                TaxTotal = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "20.00"
                                },
                                ShippingDiscount = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "10.00"
                                }
                            }
                        },
                        Items = new List<Item>
                        {
                            new Item
                            {
                                Name = "T-shirt",
                                Description = "Green XL",
                                Sku = "sku01",
                                UnitAmount = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "90.00"
                                },
                                Tax = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "10.00"
                                },
                                Quantity = "1",
                                Category = "PHYSICAL_GOODS"
                            },
                            new Item
                            {
                                Name = "Shoes",
                                Description = "Running, Size 10.5",
                                Sku = "sku02",
                                UnitAmount = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "45.00"
                                },
                                Tax = new Money
                                {
                                    CurrencyCode = "USD",
                                    Value = "5.00"
                                },
                                Quantity = "2",
                                Category = "PHYSICAL_GOODS"
                            }
                        },
                        Shipping = new ShippingDetails
                        {
                            Name = new Name
                            {
                                FullName = "John Doe"
                            },
                            AddressPortable = new AddressPortable
                            {
                                AddressLine1 = "123 Townsend St",
                                AddressLine2 = "Floor 6",
                                AdminArea2 = "San Francisco",
                                AdminArea1 = "CA",
                                PostalCode = "94107",
                                CountryCode = "US"
                            }
                        }
                    }
                }
            };

            return orderRequest;
        }

        /*
            Method to create order
            
            @param debug true = print response data
            @return HttpResponse<Order> response received from API
            @throws IOException Exceptions from API if any
        */
        public async static Task<HttpResponse> CreateOrder(bool debug = false)
        {
            var request = new OrdersCreateRequest();
            request.Headers.Add("prefer", "return=representation");
            request.RequestBody(BuildRequestBody());
            var response = await PayPalClient.client().Execute(request);

            if (debug)
            {
                var result = response.Result<Order>();
                Console.WriteLine("Status: {0}", result.Status);
                Console.WriteLine("Order Id: {0}", result.Id);
                Console.WriteLine("Intent: {0}", result.Intent);
                Console.WriteLine("Links:");
                foreach (LinkDescription link in result.Links)
                {
                    Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
                }
                AmountWithBreakdown amount = result.PurchaseUnits[0].Amount;
                Console.WriteLine("Total Amount: {0} {1}", amount.CurrencyCode, amount.Value);
            }

            return response;
        }
        /*
             This is the driver function which invokes the createOrder function to create
             an sample order.
        */
        // static void Main(string[] args)
        // {
        //     CreateOrder(true).Wait();
        // }
    }
}
