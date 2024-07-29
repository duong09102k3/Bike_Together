$(document).ready(function () {
    function getImg(images, bikes) {
        $.each(images, function (index, image) {
            console.log(image.bikesId)
            console.log(bikes.id)
            let idbikes = bikes.id;
            let idimage = image.bikesId;

            if (idbikes.toLowerCase() === idimage.toLowerCase()) {
                var imgHtml = `<img src="${image.url}" alt="Thumbnail Image">`;
                $(`.content-item[pid="${bikes.id}"] .thumbnail`).append(imgHtml);
                return false;
            } else {
                console.log('id ko băng nhau')
            }
        });
    }
    $.ajax({
        url: '/Home/GetBikesData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var contentItems = $('.content-items');
            contentItems.empty();

            $.each(data.bikes, function (index, bikes) {
                // Chuyển đổi ngày tạo ra đối tượng ngày và định dạng ngày
                var createdAtDate = new Date(bikes.createdAt);
                var formattedDate = createdAtDate.toLocaleDateString('vi-VN');

                var bikesHtml = `
                        <div pid=${bikes.id} class="content-item">
                            <div class="row">
                                <div class="ct-title">
                                    <a href="Bikes/BikesDetail?id=${bikes.id}">
                                        ${bikes.nameBike}
                                    </a>
                                </div>
                                <div class="ct-date">
                                    <a href="Bikes/BikesDetail?id=${bikes.id}">${formattedDate}</a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="thumbnail">
                                    <!-- Đây là nơi để append thẻ img -->
                                </div>
                                <div class="text">
                                    <div class="ct-brief">
                                        ${bikes.description}
                                        <span><a href="Bikes/BikesDetail?id=${bikes.id}">...xem chi tiết</a></span>
                                    </div>
                                    <div class="price-dis ">
                                        <div class="ct-price">
                                            <p class="text-bold">
                                                Giá: <span style="color: red">${bikes.price}  k/ Giờ</span>
                                            </p>
                                        </div>
                                        <div class="ct-dis">
                                            <a href="">${bikes.address}, ${bikes.fullAddress}</a>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
                contentItems.append(bikesHtml);
                getImg(data.images, bikes)
            });
        },
        error: function (err) {
            console.log('Đã xảy ra lỗi khi lấy dữ liệu!');
            console.log(err.responseText);
        }
    });

    

    $.ajax({
        url: '/Home/GetLocations',
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            var $addressul = $("<ul></ul>");
            $.each(data.locations, function (i, ele) {
                $addressul.append(
                    $("<li></li>").text(ele.name)
                );
            });

            $('.district').empty().append($addressul);

            var $priceUl = $("<ul></ul>");
            var pricesAdded = [];
            $.each(data.bikes, function (i, bikes) {
                var price = bikes.price;

                if (!pricesAdded.includes(price)) {
                    var $li = $("<li></li>").text(price);
                    var $span = $("<span></span>").text(" k");
                    $priceUl.append($li.append($span));
                    pricesAdded.push(price);
                }
            });

            $('.price').empty().append($priceUl);
            a();
        },
        error: function (err) {
            console.log('Đã xảy ra lỗi khi lấy dữ liệu!');
            console.log(err.responseText);
        }
    });
    let count = 0;
    let textSearch = '';
    function a() {
        const text_names = document.querySelectorAll(".text-name");
        const view_districts = document.querySelectorAll(".district li");
        const view_prices = document.querySelectorAll(".price li");
        const views = document.querySelectorAll(".multicolumns .view");
        const texts = document.querySelectorAll(".text-name .text");
        const icloses = document.querySelectorAll(".multicolumns .fa-times");
        const array_views = [
            view_districts,
            view_prices,
        ];
        text_names.forEach((item, index) => {
            item.onclick = function () {
                views[index].classList.remove("active-view");
                array_views[index].forEach((item) => {
                    item.onclick = function () {

                        textSearch += item.innerText + ",";
                        console.log(textSearch)
                        texts[index].innerText = item.innerText;
                        views[index].classList.add("active-view");

                    };

                });

            };
            icloses[index].onclick = function () {
                views[index].classList.add("active-view");
            };

        });

    }

    $("#button-search").click(function () {
        var keyword = textSearch + $('.location').val();
        console.log(keyword)
        $.ajax({

            url: "/Home/Search",
            type: "POST",
            data: { keyword: keyword },
            success: function (data) {
                var contentItems = $('.content-items');
                var textSeach = 'Kết quả tìm kiếm : ' + keyword;
                $('.property-type-article').css('border', 'none');
                $('.property-type-article').empty();
                contentItems.empty();

                if (data.error) {
                    $('.body .title p').first().text(data.error);
                    $('.body .title p').eq(1).text(textSeach);
                    var imgElement = $("<img>");
                    imgElement.attr("src", "/images/imageSearchErrorr.png");
                    imgElement.css({
                        "width": "99%",
                        "height": "90%",
                    });
                    contentItems.append(imgElement);
                } else {
                    $('.body .title p').first().text(textSeach);
                    $('.body .title p').eq(1).text('Danh sách kết quả tìm kiếm :');
                    $.each(data.bikes, function (index, bikes) {
                        // Chuyển đổi ngày tạo ra đối tượng ngày và định dạng ngày
                        var createdAtDate = new Date(bikes.createdAt);
                        var formattedDate = createdAtDate.toLocaleDateString('vi-VN');

                        var bikesHtml = `
                        <div pid=${bikes.id} class="content-item">
                            <div class="row">
                                <div class="ct-title">
                                    <a href="Bikes/BikesDetail?id=${bikes.id}">
                                        ${bikes.nameBike}
                                    </a>
                                </div>
                                <div class="ct-date">
                                    <a href="Bikes/BikesDetail?id=${bikes.id}">${formattedDate}</a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="thumbnail">
                                    <!-- Đây là nơi để append thẻ img -->
                                </div>
                                <div class="text">
                                    <div class="ct-brief">
                                        ${bikes.description}
                                        <span><a href="Bikes/BikesDetail?id=${bikes.id}">...xem chi tiết</a></span>
                                    </div>
                                    <div class="price-dis ">
                                        <div class="ct-price">
                                            <p class="text-bold">
                                                Giá: <span style="color: red">${bikes.price}  k/ Giờ</span>
                                            </p>
                                        </div>
                                        <div class="ct-dis">
                                            <a href="">${bikes.address}, ${bikes.fullAddress}</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;
                        contentItems.append(bikesHtml);
                        getImg(data.images, bikes)
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

//$(document).ready(function () {
//    function getImg(images, bikes) {
//        $.each(images, function (index, image) {
//            let idbikes = bikes.id;
//            let idimage = image.bikesId;

//            if (idbikes.toLowerCase() === idimage.toLowerCase()) {
//                var imgHtml = `<img src="${image.url}" alt="Thumbnail Image">`;
//                $(`.content-item[pid="${bikes.id}"] .thumbnail`).append(imgHtml);
//                return false;
//            } else {
//                console.log('id ko băng nhau');
//            }
//        });
//    }

//    $.ajax({
//        url: '/Home/GetBikesData',
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            var contentItems = $('.content-items');
//            contentItems.empty();

//            $.each(data.Bikes, function (index, bikes) {
//                var createdAtDate = new Date(bikes.CreatedAt);
//                var formattedDate = createdAtDate.toLocaleDateString('vi-VN');

//                var bikesHtml = `
//                    <div pid="${bikes.Id}" class="content-item">
//                        <div class="row">
//                            <div class="ct-title">
//                                <a href="Bikes/BikesDetail?id=${bikes.Id}">
//                                    ${bikes.NameBike}
//                                </a>
//                            </div>
//                            <div class="ct-date">
//                                <a href="Bikes/BikesDetail?id=${bikes.Id}">${formattedDate}</a>
//                            </div>
//                        </div>
//                        <div class="row">
//                            <div class="thumbnail">
//                                <!-- Đây là nơi để append thẻ img -->
//                            </div>
//                            <div class="text">
//                                <div class="ct-brief">
//                                    ${bikes.Description}
//                                    <span><a href="Bikes/BikesDetail?id=${bikes.Id}">...xem chi tiết</a></span>
//                                </div>
//                                <div class="price-dis ">
//                                    <div class="ct-price">
//                                        <p class="text-bold">
//                                            Giá: <span style="color: red">${bikes.Price} k/ Giờ</span>
//                                        </p>
//                                    </div>
//                                    <div class="ct-dis">
//                                        <a href="">${bikes.FullAddress}</a>
//                                    </div>
//                                </div>
//                            </div>
//                        </div>
//                    </div>
//                `;
//                contentItems.append(bikesHtml);
//                getImg(data.Images, bikes);
//            });
//        },
//        error: function (err) {
//            console.log('Đã xảy ra lỗi khi lấy dữ liệu!');
//            console.log(err.responseText);
//        }
//    });

//    $.ajax({
//        url: '/Home/GetLocations',
//        type: 'POST',
//        dataType: 'json',
//        success: function (data) {
//            var $addressul = $("<ul></ul>");
//            $.each(data.Locations, function (i, ele) {
//                $addressul.append(
//                    $("<li></li>").text(ele.Name)
//                );
//            });

//            $('.district').empty().append($addressul);

//            var $priceUl = $("<ul></ul>");
//            var pricesAdded = [];
//            $.each(data.Bikes, function (i, bikes) {
//                var price = bikes.Price;

//                if (!pricesAdded.includes(price)) {
//                    var $li = $("<li></li>").text(price);
//                    var $span = $("<span></span>").text(" trăm");
//                    $priceUl.append($li.append($span));
//                    pricesAdded.push(price);
//                }
//            });

//            $('.price').empty().append($priceUl);
//            a();
//        },
//        error: function (err) {
//            console.log('Đã xảy ra lỗi khi lấy dữ liệu!');
//            console.log(err.responseText);
//        }
//    });

//    let textSearch = '';
//    function a() {
//        const text_names = document.querySelectorAll(".text-name");
//        const view_districts = document.querySelectorAll(".district li");
//        const view_prices = document.querySelectorAll(".price li");
//        const views = document.querySelectorAll(".multicolumns .view");
//        const texts = document.querySelectorAll(".text-name .text");
//        const icloses = document.querySelectorAll(".multicolumns .fa-times");
//        const array_views = [
//            view_districts,
//            view_prices,
//        ];
//        text_names.forEach((item, index) => {
//            item.onclick = function () {
//                views[index].classList.remove("active-view");
//                array_views[index].forEach((item) => {
//                    item.onclick = function () {
//                        textSearch += item.innerText + ",";
//                        console.log(textSearch);
//                        texts[index].innerText = item.innerText;
//                        views[index].classList.add("active-view");
//                    };
//                });
//            };
//            icloses[index].onclick = function () {
//                views[index].classList.add("active-view");
//            };
//        });
//    }

//    $("#button-search").click(function () {
//        var keyword = textSearch + $('.location').val();
//        console.log(keyword);
//        $.ajax({
//            url: "/Home/Search",
//            type: "POST",
//            data: { keyword: keyword },
//            success: function (data) {
//                var contentItems = $('.content-items');
//                var textSeach = 'Kết quả tìm kiếm : ' + keyword;
//                $('.property-type-article').css('border', 'none');
//                $('.property-type-article').empty();
//                contentItems.empty();

//                if (data.error) {
//                    $('.body .title p').first().text(data.error);
//                    $('.body .title p').eq(1).text(textSeach);
//                    var imgElement = $("<img>");
//                    imgElement.attr("src", "/images/imageSearchErrorr.png");
//                    imgElement.css({
//                        "width": "99%",
//                        "height": "90%",
//                    });
//                    contentItems.append(imgElement);
//                } else {
//                    $('.body .title p').first().text(textSeach);
//                    $('.body .title p').eq(1).text('Danh sách kết quả tìm kiếm :');
//                    $.each(data.Bikes, function (index, bikes) {
//                        var createdAtDate = new Date(bikes.CreatedAt);
//                        var formattedDate = createdAtDate.toLocaleDateString('vi-VN');

//                        var bikesHtml = `
//                            <div pid="${bikes.Id}" class="content-item">
//                                <div class="row">
//                                    <div class="ct-title">
//                                        <a href="Bikes/BikesDetail?id=${bikes.Id}">
//                                            ${bikes.NameBike}
//                                        </a>
//                                    </div>
//                                    <div class="ct-date">
//                                        <a href="Bikes/BikesDetail?id=${bikes.Id}">${formattedDate}</a>
//                                    </div>
//                                </div>
//                                <div class="row">
//                                    <div class="thumbnail">
//                                        <!-- Đây là nơi để append thẻ img -->
//                                    </div>
//                                    <div class="text">
//                                        <div class="ct-brief">
//                                            ${bikes.Description}
//                                            <span><a href="Bikes/BikesDetail?id=${bikes.Id}">...xem chi tiết</a></span>
//                                        </div>
//                                        <div class="price-dis ">
//                                            <div class="ct-price">
//                                                <p class="text-bold">
//                                                    Giá: <span style="color: red">${bikes.Price} k/ Giờ</span>
//                                                </p>
//                                            </div>
//                                            <div class="ct-dis">
//                                                <a href="">${bikes.FullAddress}</a>
//                                            </div>
//                                        </div>
//                                    </div>
//                                </div>
//                            </div>
//                        `;
//                        contentItems.append(bikesHtml);
//                        getImg(data.Images, bikes);
//                    });
//                }
//            },
//            error: function (xhr, status, error) {
//                console.error(error);
//            }
//        });
//    });
//});
