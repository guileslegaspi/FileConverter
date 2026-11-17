namespace FileConverter.Models
{
	public class ConversionResult
	{
		public byte[] FileContent { get; set; } = Array.Empty<byte>();
		public string FileName { get; set; } = string.Empty;
		public string ContentType { get; set; } = string.Empty;
	}
}
