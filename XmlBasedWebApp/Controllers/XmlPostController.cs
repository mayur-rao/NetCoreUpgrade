using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XmlBasedWebApp.Controllers
{
	[Route("PostAwesomeXml")]
	[ApiController]
	[Route("[controller]")]
	public class XmlPostController : ControllerBase
	{
		private readonly ILogger<XmlPostController> _logger;
		private readonly OkResult okResult;

		public XmlPostController(ILogger<XmlPostController> logger)
		{
			_logger = logger;
			okResult = new OkResult();
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> CreateXmlContentAsync([FromBody] XDocument xdoc)
		{
			_logger.LogInformation("Recieved Xml Content");
			if (xdoc.Root != null)
			{
				var modifiedXml = await MessupXmlAsync(xdoc.ToString());
				_logger.LogInformation(modifiedXml);
				return okResult;
			}

			_logger.LogError("Xml is faulty. No content available");
			return new BadRequestResult();
		}

		private async Task<string> MessupXmlAsync(string xmlcontent)
		{
			using (StringReader reader = new StringReader(xmlcontent))
			{
				string readText = await reader.ReadToEndAsync();
				var messedUpXml = $"{readText}-{Guid.NewGuid()}";
				return messedUpXml;
			}
		}
	}
}
