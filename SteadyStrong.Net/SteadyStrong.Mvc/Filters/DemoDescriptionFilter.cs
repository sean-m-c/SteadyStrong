using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SteadyStrong.Mvc.Data;
using SteadyStrong.Mvc.Helpers;
using SteadyStrong.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteadyStrong.Mvc.Filters
{
    /// <summary>
    /// Runs when controller actions are executed.
    /// </summary>
    public class DemoDescriptionFilter : ActionFilterAttribute
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public DemoDescriptionFilter(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, 
            ApplicationDbContext context, 
            IConfiguration configuration
            )
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider ?? throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            if (controller == null) return;

            var showDemoDescription = false;
            Boolean.TryParse(_configuration["ShowDemoDescriptions"], out showDemoDescription);

            if(showDemoDescription)
            {
                // Add viewbag variable indicating whether showing the demo description should be
                // allowed on pages and if the current page has an entry in the demo description table.

                controller.ViewBag.ShowDemoDescription = _dbContext.DemoDescriptions.Any(d => d.PagePath == DemoDescriptionHelper.GetPageIdFromUrl(context.HttpContext.Request.Path.Value));
            }

            base.OnActionExecuted(context);
        }
    }
}
