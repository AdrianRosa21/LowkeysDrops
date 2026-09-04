package com.lowkeysdrops.web.dto;

public class PedidoCreateRequest {
    private Integer idCliente;
    private Integer idDireccion;
    private String metodoPago;

    public Integer getIdCliente() { return idCliente; }
    public void setIdCliente(Integer idCliente) { this.idCliente = idCliente; }
    public Integer getIdDireccion() { return idDireccion; }
    public void setIdDireccion(Integer idDireccion) { this.idDireccion = idDireccion; }
    public String getMetodoPago() { return metodoPago; }
    public void setMetodoPago(String metodoPago) { this.metodoPago = metodoPago; }
}
