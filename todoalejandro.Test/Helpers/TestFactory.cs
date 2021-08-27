using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using todoalejandro.Common.Models;
using todoalejandro.Functions.Entities;

namespace todoalejandro.Test.Helpers
{
    public class TestFactory
    {
        public static TodoEntity GetTodoEntity()
        {
            return new TodoEntity
            {
                ETag = "*",
                PartitionKey = "Todo",
                RowKey = Guid.NewGuid().ToString(),
                CreateTime = DateTime.UtcNow,
                IsCompleted = false,
                TaskDescription = "Task: Kill the humans."
            };
        }

        public static DefaultHttpRequest CreateHttpReques(Guid todoId, Todo todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(),
                Path = $"/{todoId}"
            };
        }

        public static DefaultHttpRequest CreateHttpReques(Guid todoId)
        {            
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{todoId}"
            };
        }

        public static DefaultHttpRequest CreateHttpReques(Todo todoRequest)
        {
            string request = JsonConvert.SerializeObject(todoRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString()
            };
        }

        public static DefaultHttpRequest CreateHttpReques()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }
        
        public static Todo GetTodoRequest() 
        {
            return new Todo
            {
                CreateTime = DateTime.UtcNow,
                IsCompleted = false,
                TaskDescription = "Try de conquer the world"
            };
        }
        
        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggersTypes type = LoggersTypes.Null)
        {
            ILogger logger;
            if (type == LoggersTypes.List)
            {
                logger = new ListLoggers();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
