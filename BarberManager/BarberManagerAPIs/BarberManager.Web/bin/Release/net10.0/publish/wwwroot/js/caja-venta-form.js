(() => {
    const formCaja = document.getElementById('formCaja');
    const ingreso = document.getElementById('ingreso');
    const egreso = document.getElementById('egreso');
    const seccionIngreso = document.getElementById('seccionIngreso');
    const seccionEgreso = document.getElementById('seccionEgreso');
    const tipoIngreso = document.getElementById('tipoIngreso');
    const ventaProducto = document.getElementById('ventaProducto');
    const producto = document.getElementById('productoVenta');
    const cantidad = document.getElementById('cantidadProducto');
    const campoOtroIngreso = document.getElementById('campoOtroIngreso');
    const detalleIngreso = document.getElementById('detalleIngreso');
    const tipoEgreso = document.getElementById('tipoEgreso');
    const detalleEgreso = document.getElementById('detalleEgreso');
    const monto = document.getElementById('Monto');
    const concepto = document.getElementById('Concepto');
    const ayudaMonto = document.getElementById('ayudaMonto');

    if (!ingreso || !egreso || !seccionIngreso || !seccionEgreso || !monto || !concepto) return;

    function productoPrecio() {
        if (!producto || producto.selectedIndex < 0) return 0;
        return Number(producto.options[producto.selectedIndex]?.dataset.precio || 0);
    }

    function actualizarMonto() {
        if (ingreso.checked && tipoIngreso && tipoIngreso.value === 'Venta de producto') {
            const unidades = Number.parseInt(cantidad?.value, 10) || 0;
            const precio = productoPrecio();
            monto.value = (precio * unidades).toFixed(2);
        }
    }

    function actualizarConcepto() {
        if (ingreso.checked) {
            const tipo = tipoIngreso ? tipoIngreso.value : 'Ingreso';
            if (tipo === 'Venta de producto') {
                const textoProd = producto && producto.selectedIndex >= 0 && producto.value
                    ? producto.options[producto.selectedIndex].text.split('·')[0].trim()
                    : '';
                concepto.value = textoProd ? `Venta de producto: ${textoProd}` : 'Venta de producto';
            } else {
                const det = detalleIngreso?.value?.trim();
                concepto.value = det ? `${tipo}: ${det}` : tipo;
            }
        } else {
            const tipo = tipoEgreso ? tipoEgreso.value : 'Egreso';
            const det = detalleEgreso?.value?.trim();
            concepto.value = det ? `${tipo}: ${det}` : tipo;
        }
    }

    function actualizarFormulario() {
        const esIngreso = ingreso.checked;

        // Alternar secciones principales
        seccionIngreso.classList.toggle('d-none', !esIngreso);
        seccionEgreso.classList.toggle('d-none', esIngreso);

        if (esIngreso) {
            const esVenta = tipoIngreso && tipoIngreso.value === 'Venta de producto';
            if (ventaProducto) ventaProducto.classList.toggle('d-none', !esVenta);
            if (campoOtroIngreso) campoOtroIngreso.classList.toggle('d-none', esVenta);

            if (producto) producto.required = esVenta;
            if (cantidad) cantidad.required = esVenta;

            if (esVenta) {
                monto.readOnly = true;
                if (ayudaMonto) {
                    ayudaMonto.classList.remove('d-none');
                    ayudaMonto.textContent = 'Se calcula automáticamente con el precio unitario y las unidades.';
                }
                actualizarMonto();
            } else {
                monto.readOnly = false;
                if (ayudaMonto) {
                    ayudaMonto.classList.remove('d-none');
                    ayudaMonto.textContent = 'Ingresá el monto total del ingreso.';
                }
            }
        } else {
            // Es Egreso
            if (producto) producto.required = false;
            if (cantidad) cantidad.required = false;
            monto.readOnly = false;
            if (ayudaMonto) {
                ayudaMonto.classList.remove('d-none');
                ayudaMonto.textContent = 'Ingresá el monto total del egreso/gasto.';
            }
        }

        actualizarConcepto();
    }

    ingreso.addEventListener('change', actualizarFormulario);
    egreso.addEventListener('change', actualizarFormulario);

    if (tipoIngreso) {
        tipoIngreso.addEventListener('change', () => {
            actualizarFormulario();
        });
    }

    if (producto) {
        producto.addEventListener('change', () => {
            actualizarMonto();
            actualizarConcepto();
        });
    }

    if (cantidad) {
        cantidad.addEventListener('input', () => {
            actualizarMonto();
            actualizarConcepto();
        });
    }

    if (detalleIngreso) {
        detalleIngreso.addEventListener('input', actualizarConcepto);
    }

    if (tipoEgreso) {
        tipoEgreso.addEventListener('change', actualizarConcepto);
    }

    if (detalleEgreso) {
        detalleEgreso.addEventListener('input', actualizarConcepto);
    }

    if (formCaja) {
        formCaja.addEventListener('submit', () => {
            actualizarConcepto();
        });
    }

    actualizarFormulario();
})();
