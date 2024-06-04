$(document).ready(function () {
    $('body').on('click', '.btn-AddToCart', function (e) {
        e.preventDefault();

        var productId = $(this).data('id');

        $.ajax({
            url: '/Cart/AddItem',
            type: 'POST',
            data: { ProductId: productId, Quantity: 1 }, // Giả sử số lượng mặc định là 1
            success: function (response) {
                if (response.Success) {
                    alert(response.Message);
                } else {
                    alert("Đã thêm sản phẩm thành công!");
                }
            },
        });
    });
});
