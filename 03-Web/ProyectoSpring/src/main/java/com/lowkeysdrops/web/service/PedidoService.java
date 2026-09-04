package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.*;
import org.springframework.stereotype.Service;

import java.util.Map;

@Service
public class PedidoService {
    
    private final ApiClient apiClient;

    public PedidoService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public PedidoCreateResponse crearPedido(PedidoCreateRequest request) {
        return apiClient.post("/api/Pedidos", request, PedidoCreateResponse.class);
    }

    public void agregarProducto(Integer idPedido, AgregarProductoRequest request) {
        apiClient.post("/api/Pedidos/" + idPedido + "/productos", request);
    }

    public PedidoResponse getById(Integer idPedido) {
        return apiClient.get("/api/Pedidos/" + idPedido, PedidoResponse.class);
    }

    public void confirmarRecepcion(Integer idPedido, Integer idCliente) {
        apiClient.put("/api/Pedidos/" + idPedido + "/confirmar-recepcion", Map.of("idCliente", idCliente));
    }
}
