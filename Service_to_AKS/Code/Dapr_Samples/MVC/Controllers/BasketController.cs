﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Dapr.Actors.Client;
using Dapr.Actors;
using MVC.Interfaces;

namespace MVC.Controllers
{
    public record Order([property: JsonPropertyName("orderId")] int orderId);
    [ApiController]
    [Route("[controller]")]
    public class BasketController : Controller
    {
        public static HttpClient httpClient = null;
        static BasketController()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public BasketController()
        {

        }

        private const string DAPR_STATE_STORE = "statestore";

        [HttpGet]
        public async Task<bool> Get()
        {
            var orderId = 100;
            var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":"
 + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");
            var order = new Order(100);
            var orderJson = JsonSerializer.Serialize(
                new[] {
            new {
                key = orderId,
                value = order
            }
                }
            );
            var state = new StringContent(orderJson, Encoding.UTF8, "application/json");


            await httpClient.PostAsync($"{baseURL}/v1.0/state/{DAPR_STATE_STORE}", state);
            Console.WriteLine("Saving Order: " + order);

            // Get state from a state store
            /*var response = await httpClient.GetAsync($"{baseURL}/v1.0/state/{DAPR_STATE_STORE}/{orderId.ToString()}");
            Console.WriteLine("Getting Order: " + response.StatusCode);

            // Delete state from the state store
            await httpClient.DeleteAsync($"{baseURL}/v1.0/state/{DAPR_STATE_STORE}/{orderId.ToString()}");
            Console.WriteLine("Deleting Order: " + order);*/

            return true;

        }

        [HttpGet("GetActor")]
        public async Task<bool> GetActor()
        {
            // Actor Ids and types
            var deviceId1 = "1";
            var deviceId2 = "2";
            var smokeDetectorActorType = "SmokeDetectorActor";
            var controllerActorType = "ControllerActor";

            Console.WriteLine("Startup up...");

            // An ActorId uniquely identifies an actor instance
            var deviceActorId1 = new ActorId(deviceId1);

            // Create a new instance of the data class that will be stored in the actor
            var deviceData1 = new SmartDeviceData()
            {
                Location = "First Floor",
                Status = "Ready",
            };

            // Create the local proxy by using the same interface that the service implements.
            // You need to provide the type and id so the actor can be located. 
            // If the actor matching this id does not exist, it will be created
            var proxySmartDevice1 = ActorProxy.Create<ISmartDevice>(deviceActorId1, smokeDetectorActorType);

            // Now you can use the actor interface to call the actor's methods.
            Console.WriteLine($"Calling SetDataAsync on {smokeDetectorActorType}:{deviceActorId1}...");
            var setDataResponse1 = await proxySmartDevice1.SetDataAsync(deviceData1);
            Console.WriteLine($"Got response: {setDataResponse1}");

            Console.WriteLine($"Calling GetDataAsync on {smokeDetectorActorType}:{deviceActorId1}...");
            var storedDeviceData1 = await proxySmartDevice1.GetDataAsync();
            Console.WriteLine($"Device 1 state: {storedDeviceData1}");

            // Create a second actor for second device
            var deviceActorId2 = new ActorId(deviceId2);
            var deviceData2 = new SmartDeviceData()
            {
                Location = "Second Floor",
                Status = "Ready",
            };
            var proxySmartDevice2 = ActorProxy.Create<ISmartDevice>(deviceActorId2, smokeDetectorActorType);
            Console.WriteLine($"Calling SetDataAsync on {smokeDetectorActorType}:{deviceActorId2}...");
            var setDataResponse2 = await proxySmartDevice2.SetDataAsync(deviceData2);
            Console.WriteLine($"Got response: {setDataResponse2}");
            Console.WriteLine($"Calling GetDataAsync on {smokeDetectorActorType}:{deviceActorId2}...");
            var storedDeviceData2 = await proxySmartDevice2.GetDataAsync();
            Console.WriteLine($"Device 2 state: {storedDeviceData2}");

            // Use the controller actor to register the device ids.
            var controllerActorId = new ActorId("controller");
            var proxyController = ActorProxy.Create<IController>(controllerActorId, controllerActorType);

            Console.WriteLine($"Registering the IDs of both Devices...");
            await proxyController.RegisterDeviceIdsAsync(new string[] { deviceId1, deviceId2 });
            var deviceIds = await proxyController.ListRegisteredDeviceIdsAsync();
            Console.WriteLine($"Registered devices: {string.Join(", ", deviceIds)}");

            // Smoke is detected on device 1 that triggers an alarm on all devices.
            Console.WriteLine($"Detecting smoke on Device 1...");
            proxySmartDevice1 = ActorProxy.Create<ISmartDevice>(deviceActorId1, smokeDetectorActorType);
            await proxySmartDevice1.DetectSmokeAsync();

            // Get the state of both devices.
            storedDeviceData1 = await proxySmartDevice1.GetDataAsync();
            Console.WriteLine($"Device 1 state: {storedDeviceData1}");
            storedDeviceData2 = await proxySmartDevice2.GetDataAsync();
            Console.WriteLine($"Device 2 state: {storedDeviceData2}");

            // Sleep for 35 seconds and observe reminders have cleared alarm state
            Console.WriteLine("Sleeping for 16 seconds before checking status again to see reminders fire and clear alarms");
            await Task.Delay(16000);

            storedDeviceData1 = await proxySmartDevice1.GetDataAsync();
            Console.WriteLine($"Device 1 state: {storedDeviceData1}");
            storedDeviceData2 = await proxySmartDevice2.GetDataAsync();
            Console.WriteLine($"Device 2 state: {storedDeviceData2}");
            return true;
        }
    
    }
}
