using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Doe_Knowledge_expert_bot.Dialogs
{
    [Serializable]
    // TODO: Enter LUIS credentials here..
    [LuisModel("", "")]
    public class RootDialog : LuisDialog<object>
    {
        [LuisIntent("Help")]
        private async Task Help(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("You can ask me things like, show me last open incident, or show me last updated active incident");
                context.Wait(MessageReceived);
            }
            catch (Exception ex)
            {
                await context.PostAsync("Something went wrong...");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Welcome")]
        private async Task Welcome(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Welcome, I'm the DOE Incident Tracker Bot, ask me things like, show me last open incident, or show me last updated active incident");
                context.Wait(MessageReceived);
            }
            catch (Exception ex)
            {
                await context.PostAsync("Something went wrong...");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Thanks")]
        private async Task Thanks(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("You're welcome human.");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Something went wrong...");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("None")]
        [LuisIntent("")]
        private async Task None(IDialogContext context, LuisResult result)
        {
            try
            {
                await context.PostAsync("Sorry, I couldn't find any incidents for that...");
                context.Wait(MessageReceived);
            }
            catch (Exception)
            {
                await context.PostAsync("Something went wrong...");
                context.Wait(MessageReceived);
            }
        }

       [LuisIntent("GetIncidents")]
       private async Task GetIncidents(IDialogContext context, LuisResult result)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(
                        // Don't judge me for this :(
                    string.Format("{0}:{1}", "Admin", ""))));

                    HttpResponseMessage response = await client.GetAsync($"https://hackathon178.service-now.com/api/now/v1/table/incident?sysparm_query=active=true^ORDERBYDESCsys_updated_on");
                    string results = await response.Content.ReadAsStringAsync();

                   //Collect information about current forecast
                    JObject json = JObject.Parse(results);
                    string incidentOpenedAt = (string)json["result"][0]["opened_at"];
                    string incidentDescription = (string)json["result"][0]["short_description"];
                    string incidentID = (string)json["result"][0]["number"];
                    string incidentUpdated = (string)json["result"][0]["sys_updated_on"];
                    string incidentState = (string)json["result"][0]["state"];
                    string incidentCreatedBy = (string)json["result"][0]["sys_created_by"];

                    if (response.IsSuccessStatusCode)
                    {
                        await context.PostAsync($"This is what i found...");
                        await context.PostAsync($"OPENED AT: - {incidentOpenedAt} LAST UPDATED: - {incidentUpdated} INCIDENT ID: - {incidentID} INCIDENT DESCRIPTION: - {incidentDescription} CREATEDBY: - {incidentCreatedBy}");
                        context.Wait(MessageReceived);
                    }

                    else if(response == null)
                    {
                        await context.PostAsync("Sorry I didn't get a response from the incident API");
                        context.Wait(MessageReceived);
                    }

                    else
                    {
                        await context.PostAsync("I didn't get a response from the API");
                        context.Wait(MessageReceived);
                    }
                }
            }
            catch (Exception ex)
            {
                await context.PostAsync(ex.StackTrace + ex.Message);
                context.Wait(MessageReceived);
            }
        }
    }
}