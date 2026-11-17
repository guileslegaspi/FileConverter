using FileConverter.Models;

namespace FileConverter.Services
{
	public class ConversionService
	{
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
