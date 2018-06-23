//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(window).bind("load resize", function () {
    var topOffset = 50;
    var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
    if (width < 768) {
        $('div.navbar-collapse').addClass('collapse');
        topOffset = 100; // 2-row-menu
    } else {
        $('div.navbar-collapse').removeClass('collapse');
    }

    var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - topOffset;
    if (height < 1) height = 1;
    if (height > topOffset) {
        $("#page-wrapper").css("min-height", (height) + "px");
    }
});

function WebsiteDropdown() {
    $('#WebsiteId').on('change', function () {
        var _this = $(this);
        var _val = $(_this).val();

        $.ajax({
            url: '/AdminHome/Index',
            type: 'POST',
            data: { websiteId: _val },
            success: function (data) {
                //document.location.reload(true);
            }
        });
    });
}