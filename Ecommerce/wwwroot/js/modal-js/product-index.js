(function ($) {
    function Product() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-product").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Product();
        self.init();
    })
}(jQuery))