namespace Framework.Http
{
    /// <summary>
    /// Common HTTP status codes
    /// </summary>
    public static class HttpStatusCodes
    {
        public const int Continue100 = 100;
        public const int SwitchingProtocols101 = 101;
        public const int Processing102 = 102;
        public const int Ok200 = 200;
        public const int Created201 = 201;
        public const int Accepted202 = 202;
        public const int NonAuthoritative203 = 203;
        public const int NoContent204 = 204;
        public const int ResetContent205 = 205;
        public const int PartialContent206 = 206;
        public const int MultiStatus207 = 207;
        public const int AlreadyReported208 = 208;
        public const int ImUsed226 = 226;
        public const int MultipleChoices300 = 300;
        public const int MovedPermanently301 = 301;
        public const int Found302 = 302;
        public const int SeeOther303 = 303;
        public const int NotModified304 = 304;
        public const int UseProxy305 = 305;
        public const int SwitchProxy306 = 306;
        public const int TemporaryRedirect307 = 307;
        public const int PermanentRedirect308 = 308;
        public const int BadRequest400 = 400;
        public const int Unauthorized401 = 401;
        public const int PaymentRequired402 = 402;
        public const int Forbidden403 = 403;
        public const int NotFound404 = 404;
        public const int MethodNotAllowed405 = 405;
        public const int NotAcceptable406 = 406;
        public const int ProxyAuthenticationRequired407 = 407;
        public const int RequestTimeout408 = 408;
        public const int Conflict409 = 409;
        public const int Gone410 = 410;
        public const int LengthRequired411 = 411;
        public const int PreconditionFailed412 = 412;
        public const int PayloadTooLarge413 = 413;
        public const int RequestEntityTooLarge413 = 413;
        public const int RequestUriTooLong414 = 414;
        public const int UriTooLong414 = 414;
        public const int UnsupportedMediaType415 = 415;
        public const int RangeNotSatisfiable416 = 416;
        public const int RequestedRangeNotSatisfiable416 = 416;
        public const int ExpectationFailed417 = 417;
        public const int ImATeapot418 = 418;
        public const int AuthenticationTimeout419 = 419;
        public const int MisdirectedRequest421 = 421;
        public const int UnprocessableEntity422 = 422;
        public const int Locked423 = 423;
        public const int FailedDependency424 = 424;
        public const int UpgradeRequired426 = 426;
        public const int PreconditionRequired428 = 428;
        public const int TooManyRequests429 = 429;
        public const int RequestHeaderFieldsTooLarge431 = 431;
        public const int UnavailableForLegalReasons451 = 451;
        public const int ClientClosedRequest499 = 499;
        public const int InternalServerError500 = 500;
        public const int NotImplemented501 = 501;
        public const int BadGateway502 = 502;
        public const int ServiceUnavailable503 = 503;
        public const int GatewayTimeout504 = 504;
        public const int HttpVersionNotSupported505 = 505;
        public const int VariantAlsoNegotiates506 = 506;
        public const int InsufficientStorage507 = 507;
        public const int LoopDetected508 = 508;
        public const int NotExtended510 = 510;
        public const int NetworkAuthenticationRequired511 = 511;
    }
}
