$(document).ready(function () {
    console.log('BookingBikeView')

    //=====================================================================================================
    //LOAD DATATABLES

    $('.contentBlock__content2').html('');

    var html = '';
    html +=
        "<table id='table' class='display'>"
        + "<thead>"
        + "<tr>"
        + "<th class='stt'>STT</th>"
        + "<th class='customerName'>Người chờ xem</th>"
        + "<th class='customerPhone'>Số điện thoại</th>"
        + "<th class='price''>Giá</th>"
        + "<th class='customerPhone'>Trạng thái</th>"
        + "<th class='action'>Thao tác</th>"
        + "</tr>"
        + "</thead>"
        + "<tbody>"

        + "</tbody>"
        + "</table>";
    $('.contentBlock__content2').append(html);

    $("#table").dataTable({
        processing: true,
        serverSide: false,
        filter: true,
        orderMulti: true,
        language: {
            info: "",
            emptyTable: "Không có dữ liệu!",
            paginate: {
                previous: "Trang trước",
                next: "Trang sau",
                first: "Trang đầu",
                last: "Trang cuối",
                search: "Tìm kiếm:"
            },
        },

        ajax: {
            url: "/BookingBike/GetBookings",
            type: "GET",
            datatype: "json"
        },

        columns: [
            {
                data: null, render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: null, render: function (data, type, row, meta) {
                    return "<span data-toggle='modal'  class='dataRow'>" + data.customerName + "</span>";
                }

            },
            {
                data: null, render: function (data, type, row, meta) {
                    return "<span data-toggle='modal'  class='dataRow'>" + data.customerPhone + "</span>";
                }

            },

            {
                data: null, render: function (data, type, row, meta) {
                    return "<span data-toggle='modal' class='dataRow'>" + data.bikePrice + "</span>";
                }

            },
            {
                data: null, render: function (data, type, row, meta) {
                    return "<span data-toggle='modal' class='dataRow'>" + data.bikeStatus + "</span>";
                }

            },

            {
                data: null, render: function (data, type, row, meta) {
                    console.log(data.customerId)
                    return "<a href='' id='btnEdit' idBookingCalender='" + data.bookingId + "' idCustomer='" + data.customerId + "' idHouse='" + data.bikeId + "' style='margin: 10px;cursor: pointer;'><i class='fas fa-play'></i></a>"
                        + "<a href='' id='btnDelete' idBookingCalender='" + data.bookingId + "' style='margin: 10px;cursor: pointer;'><i class='fas fa-trash'></i></a>";
                }
            },

        ]
    });

    //=====================================================================================================
    //MODAL EDIT
    $(document).on('click', '#btnEdit', function (e) {

        e.preventDefault();

        var requestIdBooking = $(this).attr('idBookingCalender');
        var requestIdCustomer = $(this).attr('idCustomer');
        var requestIdHouse = $(this).attr('idHouse');



        const callAPIDetail = async () => {
            try {

                $.ajax({
                    url: '/BookingBike/ChiTiet',
                    type: 'GET',
                    data: { bookingBikesId: requestIdBooking },
                    success: function (data) {
                        $('#myModalCRUD').modal('show');
                        $('.modal-title').html('Cập nhật xe');
                        $('.modal-footer').html(
                            "<button type='button' class='btn btn-primary' id='btnUpdate'>Xác nhận cho thuê</button>"
                            + "<button type = 'button' class='btn btn-default' data-dismiss='modal'>Đóng</button >"
                        );

                        $('#CustomerName').val(data[0].customerName);
                        $('#CustomerPhone').val(data[0].customerPhone);
                        $('#CustomerEmail').val(data[0].customerEmail);
                        $('#Note').val(data[0].bookingNote);
                        $('#OwnerName').val(data[0].ownerName);
                        $('#OwnerPhone').val(data[0].ownerPhone);
                        $('#NameBike').val(data[0].bikeTitle);
                        $('#LocationName').val(data[0].locationName);
                        $('#Address').val(data[0].address);
                        $('#Price').val(data[0].price);
                        $('#Description').val(data[0].description);

                        $('#CustomerName').attr("readonly", true);
                        $('#CustomerPhone').attr("readonly", true);
                        $('#CustomerEmail').attr("readonly", true);
                        $('#Note').attr("readonly", true);
                        $('#OwnerName').attr("readonly", true);
                        $('#OwnerPhone').attr("readonly", true);
                        $('#NameBike').attr("readonly", true);
                        $('#LocationName').attr("readonly", true);
                        $('#Address').attr("readonly", true);
                        $('#Price').attr("readonly", true);
                        $('#Description').attr("readonly", true);
                    },
                    error: function (xhr, status, error) {
                        console.error("Lỗi khi lấy chi tiết: " + error);
                    }
                });
            }
            catch (error) {
                console.log("Error detail: " + error);
            }
        }
        callAPIDetail();

        //var requestIdBooking = $(this).attr('idBookingCalender');
        //var requestIdCustomer = $(this).attr('idCustomer');
        //var requestIdHouse = $(this).attr('idHouse');

        //const callAPIDetail = async () => {
        //    try {
        //        $.ajax({
        //            url: '/BookingBike/ChiTiet',
        //            type: 'GET',
        //            data: { bookingBikeId: requestIdBooking },
        //            success: function (data) {
        //                console.log("Data received from API:", data); // In dữ liệu ra console để kiểm tra

        //                // Kiểm tra nếu data là một mảng và có phần tử đầu tiên
        //                if (data && Array.isArray(data) && data.length > 0) {
        //                    var bookingDetails = data[0];
        //                    $('#myModalCRUD').modal('show');
        //                    $('.modal-title').html('Cập nhật xe');
        //                    $('.modal-footer').html(
        //                        "<button type='button' class='btn btn-primary' id='btnUpdate'>Xác nhận cho thuê</button>"
        //                        + "<button type='button' class='btn btn-default' data-dismiss='modal'>Đóng</button>"
        //                    );

        //                    $('#CustomerName').val(bookingDetails.customerName);
        //                    $('#CustomerPhone').val(bookingDetails.customerPhone);
        //                    $('#CustomerEmail').val(bookingDetails.customerEmail);
        //                    $('#Note').val(bookingDetails.bookingNote);
        //                    $('#OwnerName').val(bookingDetails.ownerName);
        //                    $('#OwnerPhone').val(bookingDetails.ownerPhone);
        //                    $('#NameBike').val(bookingDetails.bikeTitle);
        //                    $('#LocationName').val(bookingDetails.locationName);
        //                    $('#Address').val(bookingDetails.address);
        //                    $('#Price').val(bookingDetails.price);
        //                    $('#Description').val(bookingDetails.description);

        //                    $('#CustomerName').attr("readonly", true);
        //                    $('#CustomerPhone').attr("readonly", true);
        //                    $('#CustomerEmail').attr("readonly", true);
        //                    $('#Note').attr("readonly", true);
        //                    $('#OwnerName').attr("readonly", true);
        //                    $('#OwnerPhone').attr("readonly", true);
        //                    $('#NameBike').attr("readonly", true);
        //                    $('#LocationName').attr("readonly", true);
        //                    $('#Address').attr("readonly", true);
        //                    $('#Price').attr("readonly", true);
        //                    $('#Description').attr("readonly", true);
        //                } else {
        //                    console.error("No valid data found");
        //                }
        //            },
        //            error: function (xhr, status, error) {
        //                console.error("Lỗi khi lấy chi tiết: " + error);
        //            }
        //        });
        //    } catch (error) {
        //        console.log("Error detail: " + error);
        //    }
        //}
        //callAPIDetail();


        $(document).on('click', '#btnUpdate', function (e) {
            let checkAction = 1;
            var answer = confirm("Bạn đã xử lý xong lịch thuê này?");
            if (answer) {

                const callAPIDelete = async () => {
                    var bikeObject = {
                        id: requestIdHouse,
                        bikeStatus: "Đang cho thuê",
                    };
                    console.log(bikeObject)
                    try {

                        await $.ajax({
                            url: '/BookingBike/UpdateBikeStatus',
                            type: 'POST',
                            contentType: 'application/json',
                            data: JSON.stringify(bikeObject),
                            success: function (data) {
                                if (checkAction == 1) {
                                    checkAction = data;
                                }
                            },
                            error: function (xhr, status, error) {
                                console.error("(UpdateBikeStatus) Chuyển trạng thái BikeStatus thất bại: " + error);
                            }
                        });
                    }
                    catch (error) {
                        console.log("Error detail: " + error);
                    }

                    try {
                        const response = await fetch(`/BookingBike/DeleteConfirmed?bookingBikesId=${requestIdBooking}`, { method: "POST" });
                        const data = await response.text();
                        if (checkAction == 1) {
                            checkAction = data;
                        }
                    } catch (error) {
                        console.log("(DeleteConfirmed) Lỗi khi cập nhật: " + error);

                    }
                };
                callAPIDelete();


                if (checkAction == 1) {
                    $('#myModalCRUD').modal('hide');
                    alert('Xác nhận xử lý thành công!');
                }
                else {
                    alert("Cập nhật thất bại!");
                }
            };
        });
    });

    //=====================================================================================================
    //DELETE FUNCTION
    $(document).on('click', '#btnDelete', function (e) {
        e.preventDefault();
        var requestIdBooking = $(this).attr('idBookingCalender');
        var requestIdCustomer = $(this).attr('idCustomer');

        let checkAction = 1;
        var answer = confirm("Bạn muốn xóa lịch đặt xe này?");
        if (answer) {

            const callAPIDelete = async () => {
                //try {
                //    const response = await fetch(`/BookingBike/RemoveBookingHouse?bookingBikesId=${requestIdBooking}&customerId=${requestIdCustomer}`, { method: "POST" });
                //    const data = await response.text();
                //    if (checkAction == 1) {
                //        checkAction = data;
                //    }
                //} catch (error) {
                //    console.log("(RemoveBookingHouse) Lỗi khi cập nhật: " + error);
                //    alert("Xóa thất bại!");

                //}


                try {
                    const response = await fetch(`/BookingBike/DeleteConfirmed?bookingBikesId=${requestIdBooking}`, { method: "POST" });
                    const data = await response.text();
                    if (checkAction == 1) {
                        checkAction = data;
                    }
                } catch (error) {
                    console.log("(DeleteConfirmed) Lỗi khi cập nhật: " + error);

                }
            };
            callAPIDelete();


            if (checkAction == 1) {
                $('#myModalCRUD').modal('hide');
                alert('Xác nhận xóa thành công!');
            }
            else {
                alert("Xóa thất bại!");
            }
        };
    });

});