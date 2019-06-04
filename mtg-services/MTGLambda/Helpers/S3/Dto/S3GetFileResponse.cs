using MTGLambda.MTGLambda.Helpers.Common;

namespace MTGLambda.MTGLambda.Helpers.S3.Dto
{
    public class S3GetFileResponse : Response
    {
        public string FilePath { get; set; }
        public string FileContent { get; set; }
    }
}
