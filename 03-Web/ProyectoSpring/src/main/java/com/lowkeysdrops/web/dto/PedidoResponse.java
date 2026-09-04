package com.lowkeysdrops.web.dto;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import java.math.BigDecimal;
import java.util.List;

@JsonIgnoreProperties(ignoreUnknown = true)
public class PedidoResponse {
    private Integer idPedido;
    private Integer idCliente;
    private Integer idDireccion;
    private String fechaPedido;
    private BigDecimal subtotal;
    private BigDecimal costoEnvio;
    private BigDecimal total;
    private String estado;
    private List<DetallePedido> detalles;
    private PagoInfo pago;
    private EntregaInfo entrega;

    // Getters and Setters
    public Integer getIdPedido() { return idPedido; }
    public void setIdPedido(Integer idPedido) { this.idPedido = idPedido; }
    public Integer getIdCliente() { return idCliente; }
    public void setIdCliente(Integer idCliente) { this.idCliente = idCliente; }
    public Integer getIdDireccion() { return idDireccion; }
    public void setIdDireccion(Integer idDireccion) { this.idDireccion = idDireccion; }
    public String getFechaPedido() { return fechaPedido; }
    public void setFechaPedido(String fechaPedido) { this.fechaPedido = fechaPedido; }
    public BigDecimal getSubtotal() { return subtotal; }
    public void setSubtotal(BigDecimal subtotal) { this.subtotal = subtotal; }
    public BigDecimal getCostoEnvio() { return costoEnvio; }
    public void setCostoEnvio(BigDecimal costoEnvio) { this.costoEnvio = costoEnvio; }
    public BigDecimal getTotal() { return total; }
    public void setTotal(BigDecimal total) { this.total = total; }
    public String getEstado() { return estado; }
    public void setEstado(String estado) { this.estado = estado; }
    public List<DetallePedido> getDetalles() { return detalles; }
    public void setDetalles(List<DetallePedido> detalles) { this.detalles = detalles; }
    public PagoInfo getPago() { return pago; }
    public void setPago(PagoInfo pago) { this.pago = pago; }
    public EntregaInfo getEntrega() { return entrega; }
    public void setEntrega(EntregaInfo entrega) { this.entrega = entrega; }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class DetallePedido {
        private Integer idDetalle;
        private Integer idProducto;
        private Integer cantidad;
        private BigDecimal precioUnitario;
        public Integer getIdDetalle() { return idDetalle; }
        public void setIdDetalle(Integer idDetalle) { this.idDetalle = idDetalle; }
        public Integer getIdProducto() { return idProducto; }
        public void setIdProducto(Integer idProducto) { this.idProducto = idProducto; }
        public Integer getCantidad() { return cantidad; }
        public void setCantidad(Integer cantidad) { this.cantidad = cantidad; }
        public BigDecimal getPrecioUnitario() { return precioUnitario; }
        public void setPrecioUnitario(BigDecimal precioUnitario) { this.precioUnitario = precioUnitario; }
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class PagoInfo {
        private Integer idPago;
        private String metodo;
        private String estado;
        private String referencia;
        private String fechaPago;
        public Integer getIdPago() { return idPago; }
        public void setIdPago(Integer idPago) { this.idPago = idPago; }
        public String getMetodo() { return metodo; }
        public void setMetodo(String metodo) { this.metodo = metodo; }
        public String getEstado() { return estado; }
        public void setEstado(String estado) { this.estado = estado; }
        public String getReferencia() { return referencia; }
        public void setReferencia(String referencia) { this.referencia = referencia; }
        public String getFechaPago() { return fechaPago; }
        public void setFechaPago(String fechaPago) { this.fechaPago = fechaPago; }
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class EntregaInfo {
        private Integer idEntrega;
        private Integer idRepartidor;
        private String estado;
        private String fechaTomado;
        private String fechaEntrega;
        public Integer getIdEntrega() { return idEntrega; }
        public void setIdEntrega(Integer idEntrega) { this.idEntrega = idEntrega; }
        public Integer getIdRepartidor() { return idRepartidor; }
        public void setIdRepartidor(Integer idRepartidor) { this.idRepartidor = idRepartidor; }
        public String getEstado() { return estado; }
        public void setEstado(String estado) { this.estado = estado; }
        public String getFechaTomado() { return fechaTomado; }
        public void setFechaTomado(String fechaTomado) { this.fechaTomado = fechaTomado; }
        public String getFechaEntrega() { return fechaEntrega; }
        public void setFechaEntrega(String fechaEntrega) { this.fechaEntrega = fechaEntrega; }
    }
}
