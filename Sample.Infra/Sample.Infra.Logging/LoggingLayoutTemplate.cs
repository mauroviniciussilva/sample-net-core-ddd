namespace Sample.Infra.Logging
{
    public class LoggingLayoutTemplate
    {
        const string WebApi = @"${longdate} 
                                - ${uppercase:${level}} 
                                - IP: ${event-properties:item=ip} 
                                - PATH: ${event-properties:item=path} 
                                - REQUEST: ${event-properties:item=requestBody} 
                                - HTTP METHOD: ${event-properties:item=httpMethod} 
                                - HOST: ${event-properties:item=host} 
                                - APP METHOD ${event-properties:item=appMethod} 
                                - STACK TRACE: ${event-properties:item=stack} 
                                - EXCEPTION MESSAGE: ${event-properties:item=exceptionMessage} 
                                - ARQUIVO: ${event-properties:item=arquivo}";

        const string ConsoleApp = @"${longdate} 
                                    - ${uppercase:${level}} 
                                    - ${exception:format=toString, Data:maxInnerExceptionLevel=5}";

        private readonly string layoutTemplate;
        public static readonly LoggingLayoutTemplate WebApiLogTemplate = new LoggingLayoutTemplate(WebApi);
        public static readonly LoggingLayoutTemplate ConsoleAppLogTemplate = new LoggingLayoutTemplate(ConsoleApp);

        private LoggingLayoutTemplate(string layoutTemplate) => this.layoutTemplate = layoutTemplate;

        public override string ToString() => layoutTemplate;
    }
}