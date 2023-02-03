using System;

namespace ItemsAdministration.Common.Infrastructure.Hosting.Descriptions;

[Serializable]
internal record ErrorResponseDescription(string Code, string Message);