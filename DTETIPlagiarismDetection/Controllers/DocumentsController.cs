using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTETIPlagiarismDetection.Models;
using System.Collections.Concurrent;

namespace DTETIPlagiarismDetection.Controllers
{
    [RoutePrefix("api/documents")]
    public class DocumentsController : ApiController
    {
        private static ConcurrentQueue<Document> currentQueue = new ConcurrentQueue<Document>();

        [Route("upload")]
        [HttpPost()]
        public IHttpActionResult SetQueue([FromBody]Document document)
        {
            Guid guid = Guid.NewGuid();
            document.DocumentId = guid;
            
            if (String.IsNullOrWhiteSpace(document.DocumentTitle) || String.IsNullOrWhiteSpace(document.Content) ||
                String.IsNullOrWhiteSpace(document.Author1))
                return BadRequest("Validation on: Document ID, Title, Contents, and First Author returned an error.");

            if (currentQueue.Count <= 10)
            {
                QueueResponse queueresp = new QueueResponse();
                queueresp.QueueId = guid.ToString();
                queueresp.Message = "Your document is successfully enqueued with ID: " + queueresp.QueueId
                    + ". Please enter this ID when checking the document processing status.";
                currentQueue.Enqueue(document);
                return Ok<QueueResponse>(queueresp);
            }
            else
                return InternalServerError();
        }

        [Route("getqueue")]
        [HttpGet()]
        public IHttpActionResult GetAllItemsInQueue()
        {
            return Ok<ConcurrentQueue<Document>>(currentQueue);
        }
    }
}
