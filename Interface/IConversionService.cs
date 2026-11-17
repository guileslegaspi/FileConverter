using FileConverter.Models;

namespace FileConverter.Interface
{
	public interface IConversionService
	{
		Task<ConversionResult> CovertWordToPdfAsync(IFormFile file);

		Task<ConversionResult> CovertPdfToWordAsync(IFormFile file);
	}
}
