package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.DropRequest;
import com.lowkeysdrops.web.dto.DropResponse;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

@Service
public class DropService {
    
    private final ApiClient apiClient;

    public DropService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public List<DropResponse> getAll() {
        return Arrays.asList(apiClient.get("/api/Drops", DropResponse[].class));
    }

    public DropResponse getById(Integer id) {
        return apiClient.get("/api/Drops/" + id, DropResponse.class);
    }

    public void create(DropRequest request) {
        apiClient.post("/api/Drops", request);
    }

    public void update(Integer id, DropRequest request) {
        apiClient.put("/api/Drops/" + id, request);
    }

    public void delete(Integer id) {
        apiClient.delete("/api/Drops/" + id);
    }
}
