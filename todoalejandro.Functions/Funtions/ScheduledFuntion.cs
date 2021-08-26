using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;
using todoalejandro.Functions.Entities;

namespace todoalejandro.Functions.Funtions
{
    public static class ScheduledFuntion
    {
        [FunctionName("ScheduledFuntion")]
        public static async Task RunAsync(
            [TimerTrigger("0 */1 * * * *")] TimerInfo myTimer,
            [Table("todo", Connection = "AzureWebJobsStorage")] CloudTable todoTable,
            ILogger log)
        {
            log.LogInformation($"Deleting completed function executed at: {DateTime.Now}");
            string filter = TableQuery.GenerateFilterConditionForBool("IsCompleted", QueryComparisons.Equal, true);
            TableQuery<TodoEntity> query = new TableQuery<TodoEntity>().Where(filter);
            TableQuerySegment<TodoEntity> completeTodos = await todoTable.ExecuteQuerySegmentedAsync(query, null);
            int deleted = 0;

            foreach (TodoEntity completedTodo in completeTodos)
            {
                await todoTable.ExecuteAsync(TableOperation.Delete(completedTodo));
                deleted++;
            }

            log.LogInformation($"Deleted: {deleted} items at: {DateTime.Now}");

        }
    }
}
