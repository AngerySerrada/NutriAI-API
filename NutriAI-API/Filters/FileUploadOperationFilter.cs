using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NutriAI_API.Filters
{
    /// <summary>
    /// Filtro de operación para Swagger que maneja correctamente la carga de archivos IFormFile
    /// </summary>
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var formFileParams = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ModelMetadata?.ModelType == typeof(IFormFile))
                .ToList();

            if (!formFileParams.Any())
                return;

            // Si hay parámetros IFormFile, configurar el request body para multipart/form-data
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>(),
                            Required = new HashSet<string>()
                        }
                    }
                }
            };

            var schema = operation.RequestBody.Content["multipart/form-data"].Schema;

            // Agregar todas las propiedades del modelo
            foreach (var param in context.ApiDescription.ParameterDescriptions)
            {
                if (param.ModelMetadata != null)
                {
                    var propertyName = param.Name;
                    
                    if (param.ModelMetadata.ModelType == typeof(IFormFile))
                    {
                        // Configurar como archivo
                        schema.Properties[propertyName] = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        };
                        schema.Required.Add(propertyName);
                    }
                    else
                    {
                        // Configurar según el tipo de dato
                        var propertySchema = new OpenApiSchema
                        {
                            Type = GetOpenApiType(param.ModelMetadata.ModelType)
                        };

                        if (param.ModelMetadata.ModelType == typeof(int) || param.ModelMetadata.ModelType == typeof(int?))
                        {
                            propertySchema.Format = "int32";
                        }
                        else if (param.ModelMetadata.ModelType == typeof(long) || param.ModelMetadata.ModelType == typeof(long?))
                        {
                            propertySchema.Format = "int64";
                        }

                        schema.Properties[propertyName] = propertySchema;

                        // Marcar como requerido si no es nullable
                        if (!param.ModelMetadata.IsNullableValueType && 
                            param.ModelMetadata.ModelType != typeof(string) &&
                            !param.ModelMetadata.ModelType.Name.Contains("Nullable"))
                        {
                            schema.Required.Add(propertyName);
                        }
                    }
                }
            }
        }

        private string GetOpenApiType(Type type)
        {
            // Manejar tipos nullable
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return underlyingType.Name switch
            {
                nameof(Int32) => "integer",
                nameof(Int64) => "integer",
                nameof(Boolean) => "boolean",
                nameof(Double) => "number",
                nameof(Decimal) => "number",
                nameof(Single) => "number",
                nameof(DateTime) => "string",
                _ => "string"
            };
        }
    }
}
