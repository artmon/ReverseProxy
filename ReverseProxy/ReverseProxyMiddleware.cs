﻿using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ReverseProxy;

public class ReverseProxyMiddleware
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private readonly RequestDelegate _nextMiddleware;
    private int _countSendBoc = 0;
    private int _countShards = 0;
    private Random _random = new Random();

    public ReverseProxyMiddleware(RequestDelegate nextMiddleware)
    {
        _nextMiddleware = nextMiddleware;
    }

    public async Task Invoke(HttpContext context)
    {
        var targetUri = new Uri("http://127.0.0.1" + context.Request.Path + context.Request.QueryString); //BuildTargetUri(context.Request);    
        //var targetUri = new Uri("http://127.0.0.1:8081/jsonRPC"); //BuildTargetUri(context.Request);

        if (targetUri != null)
        {
            string requestBody;

            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            var reader = await context.Request.BodyReader.ReadAsync();
            var buffer = reader.Buffer;
            var body = Encoding.UTF8.GetString(buffer.FirstSpan);
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(body))
            {
                var json = JsonSerializer.Deserialize<RequestBody>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
                //
                // if (json.method.Equals("SendBoc", StringComparison.InvariantCultureIgnoreCase))
                // {
                //     _countSendBoc++;
                //
                //     // if (_countSendBoc % _random.Next(1, 6) != 0)
                //     // {
                //         var bytes = 
                //             Encoding.UTF8.GetBytes("""{"ok":false,"error":"LITE_SERVER_UNKNOWN: cannot apply external message to current state : External message was not accepted\nCannot run message on account: inbound external message rejected by transaction E1ECB7C102EA3DB3ADD7713CF3058D93A6BAD8BD6B38F83747ACEB76736A5D6D:\nexitcode=35, steps=20, gas_used=0\nVM Log (truncated):\n...ute IFJMP\nexecute INC\nexecute THROWIF 32\nexecute PUSHPOW2 9\nexecute LDSLICEX\nexecute DUP\nexecute LDU 32\nexecute LDU 32\nexecute LDU 32\nexecute NOW\nexecute XCHG s1,s3\nexecute LEQ\nexecute THROWIF 35\ndefault exception handler, terminating vm with exit code 35\n","code":500}""");
                //             //Encoding.UTF8.GetBytes("""{"ok":true,"result":{"@type":"ok","@extra":"1697718487.2204423:0:0.08746014090898802"},"jsonrpc":"2.0","id":"1"}""");
                //         await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                //         return;
                //     // }
                // }
                //

                if (json.method.Equals("getTransactions", StringComparison.InvariantCultureIgnoreCase) 
                    && json.@params.address.Equals("EQDCUeT25s9kTtKZnwR1MjVChUvlyOpJxqrBKnBSed8MCYft") )
                    // && json.@params.lt > 0)
                {
                    // var bytes =
                    //     Encoding.UTF8.GetBytes("""{"ok":false,"error":"LITE_SERVER_UNKNOWN: cannot locate transaction in block with specified logical time","code":500}""");
                    // //Encoding.UTF8.GetBytes("""{"ok":true,"result":{"@type":"ok","@extra":"1697718487.2204423:0:0.08746014090898802"},"jsonrpc":"2.0","id":"1"}""");
                    // await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    // return;
                }

                // if (json.method.Equals("shards", StringComparison.InvariantCultureIgnoreCase))
                // {
                //     _countShards++;
                //
                //     if (_countSendBoc % 200 != 0)
                //     {
                //         var bytes = Encoding.UTF8.GetBytes(
                //             """{"ok":true,"result":{"@type":"blocks.shards","shards":[{"@type":"ton.blockIdExt","workchain":0,"shard":"-9223372036854775808","seqno":15264075,"root_hash":"ac3LqPDNTS0/1TqAPswX39JQqbXzzM+A/Q8rwWWa28E=","file_hash":"xHbEhOhs8hRe1ibdx84XcTG7gva1V4N+RTJ21GLgKdc="}],"@extra":"1698239281.1026263:0:0.89665821804565"},"jsonrpc":"2.0","id":"1"}""");
                //         await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                //         return;
                //     }
                // }

                // if (json.method.Equals("estimateFee", StringComparison.InvariantCultureIgnoreCase))
                // {
                //     var bytes = Encoding.UTF8.GetBytes(
                //         """{"ok":true,"result":{"@type":"query.fees","source_fees":{"@type":"fees","in_fwd_fee":603200,"storage_fee":1,"gas_fee":0,"fwd_fee":0},"destination_fees":[],"@extra":"1714044475.2637854:1:0.0023233219538612015"},"jsonrpc":"2.0","id":"1"}""");
                //     await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                //     return;
                // }
            }

            // if (context.Request.Path.ToString().Contains("getMasterchainInfo"))
            // {
            //     context.Response.StatusCode = 500;
            //     var bytes = Encoding.UTF8.GetBytes("""{"ok":false,"error":"LITE_SERVER_UNKNOWN: cannot locate transaction in block with specified logical time","code":500}""");
            //     await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            //     return;
            // }
            //

            // if (context.Request.Path.ToString().Contains("getBlockTransactions"))
            // {
            //     context.Response.StatusCode = 500;
            //     var bytes = Encoding.UTF8.GetBytes("""
            //                                        {
            //                                          "ok": false,
            //                                          "error": "LITE_SERVER_NOTREADY: cannot find block (0,6000000000000000) seqno=21372799: ltdb: block not found (possibly out of sync: shard_client_seqno=19901500 ls_seqno=19901500)",
            //                                          "code": 500
            //                                        }
            //                                        """);
            //     await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            //     return;
            // }
            //
            //
            // if (context.Request.Path.ToString().Contains("shards"))
            // {
            //     context.Response.StatusCode = 500;
            //     var bytes = Encoding.UTF8.GetBytes("""
            //                                        {
            //                                          "ok": false,
            //                                          "error": "LITE_SERVER_NOTREADY: cannot find block (-1,8000000000000000) seqno=19902557: ltdb: block not found (last known masterchain block: 19901560)",
            //                                          "code": 500
            //                                        }
            //                                        """);
            //     await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            //     return;
            // }


            var targetRequestMessage = CreateTargetMessage(context, targetUri);

            using (var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
            {
                context.Response.StatusCode = (int)responseMessage.StatusCode;
                CopyFromTargetResponseHeaders(context, responseMessage);
                await responseMessage.Content.CopyToAsync(context.Response.Body);
            }

            return;
        }

        await _nextMiddleware(context);
    }

    private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
    {
        var requestMessage = new HttpRequestMessage();
        CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

        requestMessage.RequestUri = targetUri;
        requestMessage.Headers.Host = targetUri.Host;
        requestMessage.Method = GetMethod(context.Request.Method);

        return requestMessage;
    }

    private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
    {
        var requestMethod = context.Request.Method;

        if (!HttpMethods.IsGet(requestMethod) &&
            !HttpMethods.IsHead(requestMethod) &&
            !HttpMethods.IsDelete(requestMethod) &&
            !HttpMethods.IsTrace(requestMethod))
        {
            var streamContent = new StreamContent(context.Request.Body);
            requestMessage.Content = streamContent;
        }

        foreach (var header in context.Request.Headers)
        {
            requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }
    }

    private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
    {
        foreach (var header in responseMessage.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        foreach (var header in responseMessage.Content.Headers)
        {
            context.Response.Headers[header.Key] = header.Value.ToArray();
        }

        context.Response.Headers.Remove("transfer-encoding");
    }

    private static HttpMethod GetMethod(string method)
    {
        if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
        if (HttpMethods.IsGet(method)) return HttpMethod.Get;
        if (HttpMethods.IsHead(method)) return HttpMethod.Head;
        if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
        if (HttpMethods.IsPost(method)) return HttpMethod.Post;
        if (HttpMethods.IsPut(method)) return HttpMethod.Put;
        if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
        return new HttpMethod(method);
    }

    private Uri BuildTargetUri(HttpRequest request)
    {
        Uri targetUri = null;

        if (request.Path.StartsWithSegments("/googleforms", out var remainingPath))
        {
            targetUri = new Uri("https://docs.google.com/forms" + remainingPath);
        }

        return targetUri;
    }
}