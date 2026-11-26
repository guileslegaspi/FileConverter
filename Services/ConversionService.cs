using FileConverter.Configuration;
using FileConverter.Interface;
using FileConverter.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;

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
			var temporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

			Directory.CreateDirectory(temporaryDirectory);

			try
			{
				var safeFileName = Path.GetFileName(file.FileName);
				var inputPath = Path.Combine(temporaryDirectory, safeFileName);

				var outputPath = Path.Combine
					(
						temporaryDirectory,
						Path.GetFileNameWithoutExtension(file.FileName) + ".pdf"
					);

				await using (var stream = new FileStream(inputPath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				var startInfo = new ProcessStartInfo
				{
					FileName = _settings.ExecutablePath,
					Arguments = $"--headless --convert-to pdf --outdir \"{temporaryDirectory}\" \"{inputPath}\"",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				};

				using var process = new Process { StartInfo = startInfo };

				process.Start();

				var output = await process.StandardOutput.ReadToEndAsync();
				var error = await process.StandardError.ReadToEndAsync();

				await process.WaitForExitAsync();

				if (process.ExitCode != 0)
				{
					throw new Exception($"Conversion failed: {error} {output}");
				}

				var fileContent = await File.ReadAllBytesAsync(outputPath);

				return new ConversionResult
				{
					FileContent = fileContent,
					FileName = Path.GetFileName(outputPath),
					ContentType = "application/pdf"
				};


			}
			finally 
			{
				if(Directory.Exists(temporaryDirectory))
				{
					Directory.Delete(temporaryDirectory, true);
				}
			}



		}

		public async Task<ConversionResult> CovertPdfToWordAsync(IFormFile file)
		{
			var temporaryDirectory = Path.Combine(
				Path.GetTempPath(),
				Guid.NewGuid().ToString()
			);

			Directory.CreateDirectory(temporaryDirectory);

			try
			{
				var safeFileName = Path.GetFileName(file.FileName);

				var inputPath = Path.Combine(temporaryDirectory, safeFileName);

				var outputPath = Path.Combine(
					temporaryDirectory,
					Path.GetFileNameWithoutExtension(safeFileName) + ".docx"
				);

				await using (var stream = new FileStream(inputPath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				var startInfo = new ProcessStartInfo
				{
					FileName = _settings.ExecutablePath,
					Arguments = $"--headless --infilter=\"writer_pdf_import\" --convert-to docx:\"Office Open XML Text\" --outdir \"{temporaryDirectory}\" \"{inputPath}\"",
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				};

				using var process = new Process { StartInfo = startInfo };

				process.Start();

				var output = await process.StandardOutput.ReadToEndAsync();
				var error = await process.StandardError.ReadToEndAsync();

				await process.WaitForExitAsync();

				if (process.ExitCode != 0 || !File.Exists(outputPath))
				{
					throw new Exception($"Conversion failed: {error} {output}");
				}

				var fileContent = await File.ReadAllBytesAsync(outputPath);

				return new ConversionResult
				{
					FileContent = fileContent,
					FileName = Path.GetFileName(outputPath),
					ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
				};
			}
			finally 
			{
				if(Directory.Exists(temporaryDirectory))
				{
					Directory.Delete(temporaryDirectory, true);
				}
			}

		}
	}
}
