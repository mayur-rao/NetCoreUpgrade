using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace XmlBasedWebApp.Formatter
{
	public class XmlInputFormatter : InputFormatter
	{
		public XmlInputFormatter()
		{
			SupportedMediaTypes.Add("application/xml");
		}
		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
		{
			var xmlRequest = context.HttpContext.Request;
			using (var xmlStream = new StreamReader(xmlRequest.Body))
			{
				var xmlDoc = await XDocument.LoadAsync(xmlStream, LoadOptions.None, CancellationToken.None);
				return await InputFormatterResult.SuccessAsync(xmlDoc);
			}
		}
	}
}
