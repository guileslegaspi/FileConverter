using FileConverter.Configuration;
using FileConverter.Interface;
using FileConverter.Models;
using Microsoft.Extensions.Options;

namespace FileConverter.Services
{
	public class ConversionService : IConversionService
 	{
		private readonly LibreOfficeSettings _settings;
		public ConversionService(IOptions<LibreOfficeSettings> options)
		{
			_settings = options.Value;
		}

		public async Task<ConversionResult> CovertWordToPdfAsync(IFormFile file)
		{

			throw new NotImplementedException("Word to PDF conversion is not implemented yet.");

		}

		public async Task<ConversionResult> CovertPdfToWordAsync(IFormFile file)
		{
			throw new NotImplementedException("PDF to Word conversion is not implemented yet.");
		}
	}
}
