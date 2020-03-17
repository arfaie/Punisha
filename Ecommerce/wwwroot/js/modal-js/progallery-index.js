(function ($) {
    function ProductGallery() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-progallery").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new ProductGallery();
        self.init();
    })
}(jQuery))