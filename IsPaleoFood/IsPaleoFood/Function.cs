using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace IsPaleoFood
{
    public class Function
    {
        //else if (requestType == typeof(AudioPlayerRequest))
        //{
        //    // do some audio response stuff
        //}

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // check what type of a request it is like an IntentRequest or a LaunchRequest
            var requestType = input.GetRequestType();
            var speech = new SsmlOutputSpeech();

            var finalResponse = new SkillResponse();

            if (requestType == typeof(LaunchRequest))
            {
                input.Session.Attributes = new Dictionary<string, object>
                {
                    { "paleofoodlist", "false" },
                    {"startpaleodiet","false" },
                    { "paleoActivities","false"}
                };
                //context.Logger.Log(input.Session.Attributes["paleofoodlist"].ToString());
                finalResponse = LaunchResponse(input, speech);
            }
            else if (requestType == typeof(IntentRequest))
            {
                // do some intent-based stuff
                var intentRequest = input.Request as IntentRequest;

                context.Logger.Log($"What is the intent name:  {intentRequest.Intent.Name.ToString()}");

                // check the name to determine what you should do
                if (intentRequest.Intent.Name.ToString().Equals("explainPalexo"))
                {
                    finalResponse = ExplainPalexo(input, context, intentRequest, speech);
                }
                else if (intentRequest.Intent.Name.ToString().Equals("firstStep"))
                {
                    finalResponse = FirstStep(input, context, intentRequest, speech);
                }
                else if (intentRequest.Intent.Name.ToString().Equals("yesPalexoIntent"))
                {
                    finalResponse = YesIntent(input, context, intentRequest, speech);
                }
                else if (intentRequest.Intent.Name.ToString().Equals("checkFood"))
                {
                    finalResponse = CheckFoodFunction(input, context, intentRequest, speech);
                }
                else if (intentRequest.Intent.Name.ToString().Equals("allPaleoFood"))
                {
                    finalResponse = AllPaleoFood(input, intentRequest, speech);
                }
                else if (intentRequest.Intent.Name.ToString().Equals("paleoActivities"))
                {
                    finalResponse = PaleoActivities(input, intentRequest, speech);
                }
                else
                {
                    finalResponse = DefaultResponse(input, speech);
                }
            }
            else
            {
                finalResponse = DefaultResponse(input, speech);
            }
            return finalResponse;
        }

        private static SkillResponse LaunchResponse(SkillRequest input, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // create the speech response - cards still need a voice response
            speech.Ssml = "<speak>Welcome to Paleo diet coach. I'm an Alexa skill set to help you to understand about the paleo diet. You can try asking me what is paleo diet?</speak>";

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "How can I help you?";

            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody, input.Session);
            return finalResponse;
        }

        private static SkillResponse DefaultResponse(SkillRequest input, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;

            // create the speech response - cards still need a voice response
            speech.Ssml = $"<speak>You're smart! But please ask right questions to me! Like- {GetRandomResponse(true)}</speak>";

            // create the response
            finalResponse = ResponseBuilder.Tell(speech, input.Session);

            return finalResponse;
        }

        private SkillResponse ExplainPalexo(SkillRequest input, ILambdaContext context, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // get the slots
            // create the speech response - cards still need a voice response
            speech.Ssml = $"<speak>A proven diet plan, helps you to reduce your weight in 90 days! To know more about paleo please visit offical Facebook Paleo C U G group. Would you like to know how to start the paleo diet?</speak>";

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Would you like to know how to start the paleo diet?";

            //context.Logger.Log(input.Session.Attributes.ToString());

            foreach (var item in input.Session.Attributes.Values)
            {
                context.Logger.Log(item.ToString());
            }

            input.Session.Attributes["startpaleodiet"] = "true";
            //var yesIntentValuePaleofoodlist = Convert.ToBoolean(input.Session.Attributes["paleofoodlist"]);
            //var yesIntentValueStartPaleoDiet = Convert.ToBoolean(input.Session.Attributes["startpaleodiet"]);

            //context.Logger.Log("ExplainPaleo "+yesIntentValuePaleofoodlist.ToString());
            //context.Logger.Log("ExplainPaleo " + yesIntentValueStartPaleoDiet.ToString());

            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody, input.Session);
            return finalResponse;
        }

        private SkillResponse FirstStep(SkillRequest input, ILambdaContext context, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // get the slots
            // create the speech response - cards still need a voice response
            speech.Ssml = $"<speak>As a first step, " +
                $"visit a nearest blood test center to undergo the paleo blood test " +
                $"and post your reports in the official Facebook page! " +
                $"Volunteers will respond back with a diet chart based on your reports. Would you like to know the list of paleo foods?</speak>";

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Would you like to know the list of paleo foods?";

            input.Session.Attributes["paleofoodlist"] = "true";
            //var yesIntentValuePaleofoodlist = Convert.ToBoolean(input.Session.Attributes["paleofoodlist"]);
            //var yesIntentValueStartPaleoDiet = Convert.ToBoolean(input.Session.Attributes["startpaleodiet"]);

            //context.Logger.Log("FirstStep " + yesIntentValuePaleofoodlist.ToString());
            //context.Logger.Log("FirstStep " + yesIntentValueStartPaleoDiet.ToString());

            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody,input.Session);
            
            return finalResponse;
        }

        private SkillResponse YesIntent(SkillRequest input, ILambdaContext context, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            var yesIntentValuePaleofoodlist = Convert.ToBoolean(input.Session.Attributes["paleofoodlist"]);
            var yesIntentValueStartPaleoDiet = Convert.ToBoolean(input.Session.Attributes["startpaleodiet"]);
            var yesIntentValuePaleoActivities = Convert.ToBoolean(input.Session.Attributes["paleoActivities"]);

            //context.Logger.Log(yesIntentValuePaleofoodlist.ToString());
            //context.Logger.Log(yesIntentValueStartPaleoDiet.ToString());

            if (yesIntentValuePaleofoodlist)
            {
                input.Session.Attributes["paleofoodlist"] = "false";
                input.Session.Attributes["startpaleodiet"] = "false";
                input.Session.Attributes["paleoActivities"] = "false";


                return AllPaleoFood(input, intentRequest, speech);
            }
            else if (yesIntentValueStartPaleoDiet)
            {
                input.Session.Attributes["paleofoodlist"] = "false";
                input.Session.Attributes["startpaleodiet"] = "false";
                input.Session.Attributes["paleoActivities"] = "false";


                return FirstStep(input, context, intentRequest, speech);
            }
            else if (yesIntentValuePaleoActivities)
            {
                input.Session.Attributes["paleofoodlist"] = "false";
                input.Session.Attributes["startpaleodiet"] = "false";
                input.Session.Attributes["paleoActivities"] = "false";

                return PaleoActivities(input, intentRequest, speech);
            }
            else
            {
                return DefaultResponse(input, speech);
            }
        }

        private SkillResponse AllPaleoFood(SkillRequest input, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // get the slots

            // create the speech response - cards still need a voice response
            var foodlist = string.Empty;
            foreach (var item in GetPaleoFoods())
            {
                foodlist += item + ",";
            }
            foodlist = foodlist.Remove(foodlist.Length - 1, 1);
            var paleofoodlist = $"These are the paleo foods: {foodlist}";
            speech.Ssml = $"<speak>{paleofoodlist}! Would you like to know more about paleo activities?</speak>";

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Would you like to know more paleo about activities?";

            input.Session.Attributes["paleoActivities"] = "true";


            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody, input.Session);

            return finalResponse;
        }

        private SkillResponse PaleoActivities(SkillRequest input, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // get the slots

            // create the speech response - cards still need a voice response
            speech.Ssml = $"<speak>{GetRandomResponse(false)}! Still you need to know more?</speak>";

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Still you need to know more?";

            input.Session.Attributes["paleoActivities"] = "true";


            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody, input.Session);

            return finalResponse;
        }

        private SkillResponse CheckFoodFunction(SkillRequest input, ILambdaContext context, IntentRequest intentRequest, SsmlOutputSpeech speech)
        {
            SkillResponse finalResponse;
            // get the slots
            var foodName = intentRequest.Intent.Slots["foodname"].Value;
            context.Logger.Log($"What is it {foodName}");

            if (IsPaleoFood(foodName))
            {
                // create the speech response - cards still need a voice response
                speech.Ssml = "<speak>Yes. No limit but only once in a day! Would you like to know more about paleo activities?</speak>";
            }
            else
            {
                // create the speech response - cards still need a voice response
                speech.Ssml = "<speak>No! It is not paleo food. Would you like to know more about paleo activities?</speak>";
            }

            // create the speech reprompt
            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Would you like to know more paleo activities?";

            input.Session.Attributes["paleoActivities"] = "true";

            // create the reprompt
            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            // create the response
            finalResponse = ResponseBuilder.Ask(speech, repromptBody, input.Session);

            return finalResponse;
        }

        private bool IsPaleoFood(string foodname)
        {
            if (GetPaleoFoods().Any(s => s.Equals(foodname, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> GetPaleoFoods()
        {
            return new List<string>
            {
                "Vegetables with non-tubeless, non-beans, nuts",
                "Badam",
                "Pista",
                "Magadamia",
                "Walnuts",
                "Eggs with yolk",
                "Unsolicited meat varieties with cholesterol",
                "All kinds of seafood",
                "Chicken",
                "Ghee",
                "Butter",
                "Paneer",
                "Cheese",
                "Bananas",
                "Whole milk",
                "Yogurt",
                "Buttermilk",
                "Pure coconut oil",
                "All types of greens"
            };
        }

        private static string GetRandomResponse(bool isQuestion)
        {
            var random = new Random();

            if (!isQuestion)
            {
                var otherPaleoActivities = GetOtherPaleoActivities();
                return otherPaleoActivities.ElementAt(random.Next(otherPaleoActivities.Count));
            }
            else
            {
                var otherRequests = GetOtherRequests();
                return otherRequests.ElementAt(random.Next(otherRequests.Count));
            }
        }

        private static List<string> GetOtherRequests()
        {
            return new List<string>
            {
                "Can you check badam is paleo or not?",
                "What is paleo diet",
                "List all paleo food",
                "How can I start the paleo diet",
                "what are all the other paleo activities"
            };
        }

        private static List<string> GetOtherPaleoActivities()
        {
            return new List<string>
            {
                "You should do 45 minutes of exercises like walking, jogging every day!",
                "You can have one day after 30 days as a cheating day",
                "Do not sit idle for more than 1 hour",
                "Have sunbath from 12pm to 1 pm to absorb Vitamin D directly from sun",
                "Have your breakfast before 9am. Badam or egg or any green is preferable",
                "Have enough veggies every day",
                "You should have non veg meal at least once in a day. Preferably as lunch. Like grilled chicken, fat meat",
                "You can have coconut, guava as every day snacks",
                "Drink at least 3 litres of water every day",
                "Lemon drink is allowed as a paleo drink",
                "During initial days take turmeric medicine. Reach Facebook page for more details",
                "Check your weight at least once in a week",
                "Do not skip any medicines that you used to take before this diet without doctor's advice",
                "Please don't smoke cigarattes during this diet",
                "Avoid alchocol consumption during this diet",
                "You can check calories of the food in https://cronometer.com/ website",
                "There are non veg and veg recipes available in Amazon Kindle store."
            };
        }

    }
}
