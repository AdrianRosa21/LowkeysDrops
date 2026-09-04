package com.lowkeysdrops.web.client;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.lowkeysdrops.web.exception.ApiException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpSession;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatusCode;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Component;
import org.springframework.web.client.RestClient;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

@Component
public class ApiClient {

    private final RestClient restClient;
    private final ObjectMapper objectMapper;

    public ApiClient(@Value("${lowkeys.api.base-url}") String baseUrl, ObjectMapper objectMapper) {
        this.objectMapper = objectMapper;
        this.restClient = RestClient.builder()
                .baseUrl(baseUrl)
                .defaultStatusHandler(HttpStatusCode::isError, (request, response) -> {
                    String errorBody = new String(response.getBody().readAllBytes());
                    String errorMessage = extractErrorMessage(errorBody);
                    throw new ApiException(response.getStatusCode().value(), errorMessage);
                })
                .build();
    }

    private String extractErrorMessage(String errorBody) {
        try {
            JsonNode root = objectMapper.readTree(errorBody);
            // Handle ValidationProblemDetails (e.g., .NET 400 Bad Request with "errors")
            if (root.has("errors")) {
                JsonNode errors = root.get("errors");
                StringBuilder sb = new StringBuilder();
                if (root.has("title")) {
                    sb.append(root.get("title").asText()).append(" ");
                }
                errors.fields().forEachRemaining(entry -> {
                    JsonNode array = entry.getValue();
                    if (array.isArray() && !array.isEmpty()) {
                        sb.append(array.get(0).asText()).append(" ");
                    }
                });
                return sb.toString().trim();
            }
            // Handle simple custom error format: { "status": 409, "message": "..." }
            if (root.has("message")) {
                return root.get("message").asText();
            }
            if (root.has("title")) {
                return root.get("title").asText();
            }
            return "Ocurrió un error en la solicitud a la API.";
        } catch (Exception e) {
            // Not a JSON or unparseable
            return errorBody.isEmpty() ? "Error desconocido." : errorBody;
        }
    }

    private String getToken() {
        ServletRequestAttributes attr = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        if (attr != null) {
            HttpServletRequest request = attr.getRequest();
            HttpSession session = request.getSession(false);
            if (session != null) {
                return (String) session.getAttribute("AUTH_TOKEN");
            }
        }
        return null;
    }

    public <T> T get(String uri, Class<T> responseType) {
        String token = getToken();
        return restClient.get()
                .uri(uri)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .retrieve()
                .body(responseType);
    }

    public <T> T post(String uri, Object body, Class<T> responseType) {
        String token = getToken();
        return restClient.post()
                .uri(uri)
                .contentType(MediaType.APPLICATION_JSON)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .body(body)
                .retrieve()
                .body(responseType);
    }

    public void post(String uri, Object body) {
        String token = getToken();
        restClient.post()
                .uri(uri)
                .contentType(MediaType.APPLICATION_JSON)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .body(body)
                .retrieve()
                .toBodilessEntity();
    }

    public <T> T put(String uri, Object body, Class<T> responseType) {
        String token = getToken();
        return restClient.put()
                .uri(uri)
                .contentType(MediaType.APPLICATION_JSON)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .body(body)
                .retrieve()
                .body(responseType);
    }
    
    public void put(String uri, Object body) {
        String token = getToken();
        restClient.put()
                .uri(uri)
                .contentType(MediaType.APPLICATION_JSON)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .body(body)
                .retrieve()
                .toBodilessEntity();
    }

    public void delete(String uri) {
        String token = getToken();
        restClient.delete()
                .uri(uri)
                .headers(headers -> {
                    if (token != null) {
                        headers.setBearerAuth(token);
                    }
                })
                .retrieve()
                .toBodilessEntity();
    }
}
