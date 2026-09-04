using Microsoft.OpenApi;
using System.Collections.Generic;

class Test
{
    void M()
    {
        var req = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                },
                new List<string>()
            }
        };
    }
}
