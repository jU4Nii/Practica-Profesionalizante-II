(() => {
    const ingreso = document.getElementById('ingreso');
    const egreso = document.getElementById('egreso');
    const tipoIngreso = document.getElementById('tipoIngreso');
    const ventaProducto = document.getElementById('ventaProducto');
    const producto = document.getElementById('productoVenta');
    const cantidad = document.getElementById('cantidadProducto');
    const monto = document.getElementById('Monto');
    const concepto = document.getElementById('Concepto');
    const campoEgreso = document.getElementById('campoEgreso');
    const detalleEgreso = document.getElementById('detalleEgreso');
    const ayudaMonto = document.getElementById('ayudaMonto');
    if (!ingreso || !egreso || !tipoIngreso || !ventaProducto || !producto || !cantidad || !monto || !concepto || !campoEgreso || !detalleEgreso || !ayudaMonto) return;

    function productoActual() {
        return Number(producto.options[producto.selectedIndex]?.dataset.precio || 0);
    }

    function actualizarMonto() {
        const unidades = Number.parseInt(cantidad.value, 10) || 0;
        monto.value = (productoActual() * unidades).toFixed(2);
    }

    function actualizarFormulario() {
        const esVentaProducto = ingreso.checked && tipoIngreso.value === 'Venta de producto';
        ventaProducto.classList.toggle('d-none', !esVentaProducto);
        campoEgreso.classList.toggle('d-none', ingreso.checked);
        producto.required = esVentaProducto;
        cantidad.required = esVentaProducto;
        detalleEgreso.required = egreso.checked;
        monto.readOnly = esVentaProducto;
        ayudaMonto.classList.toggle('d-none', !esVentaProducto);
        concepto.value = esVentaProducto ? 'Venta de producto' : (ingreso.checked ? tipoIngreso.value : detalleEgreso.value);
        if (esVentaProducto) actualizarMonto();
    }

    ingreso.addEventListener('change', actualizarFormulario);
    egreso.addEventListener('change', actualizarFormulario);
    tipoIngreso.addEventListener('change', actualizarFormulario);
    producto.addEventListener('change', actualizarMonto);
    cantidad.addEventListener('input', actualizarMonto);
    detalleEgreso.addEventListener('input', actualizarFormulario);
    actualizarFormulario();
})();
