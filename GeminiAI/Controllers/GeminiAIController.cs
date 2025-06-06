﻿using GenAIWithGemini.Client;
using GenAIWithGemini.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Web;

namespace GenAIWithGemini.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeminiAIController : ControllerBase
    {
        private readonly GeminiApiClient _geminiApiClient;

        public GeminiAIController(GeminiApiClient geminiApiClient)
        {
            _geminiApiClient = geminiApiClient;
        }

        [HttpPost("check-text")]
        public async Task<IActionResult> CheckText([FromBody] PromptRequest request)
        {
            try
            {
                string safetyCheckPrompt = "Nội dung sau có chứa thông tin hoặc từ nhạy cảm hay không, nếu có thông tin hoặc từ nhạy cảm thì trả lời là có còn không nhạy cảm thì trả lời là không:";
                string finalPrompt = safetyCheckPrompt + request.Prompt;

                string response = await _geminiApiClient.CheckTextAsync(finalPrompt);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("find-text-error")]
        public async Task<IActionResult> FindTextError([FromBody] PromptRequest request)
        {
            try
            {

                string decodedPrompt = HttpUtility.UrlDecode(request.Prompt);

                string plainText = Regex.Replace(decodedPrompt, "<.*?>", string.Empty);

                string safetyCheckPrompt = "Nội dung sau có chứa thông tin hoặc từ ngữ nhạy cảm hay không, nếu có thông tin hoặc từ ngữ nhạy cảm thì hãy liệt kê ra những chữ đó và không cần giải thích gì hết:  ";
                string finalPrompt = safetyCheckPrompt + request.Prompt;

                string response = await _geminiApiClient.CheckTextAsync(finalPrompt);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost("check-image")]
        //public async Task<IActionResult> CheckImage([FromForm] IFormFile imageFile)
        //{
        //    try
        //    {
        //        if (imageFile == null || imageFile.Length == 0)
        //            return BadRequest("No image uploaded.");

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await imageFile.CopyToAsync(memoryStream);
        //            byte[] imageBytes = memoryStream.ToArray();
        //            string base64Image = Convert.ToBase64String(imageBytes);

        //            string analysisResult = await _geminiApiClient.AnalyzeImageAsync(base64Image);
        //            return Ok(new { result = analysisResult });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}

    }
}
