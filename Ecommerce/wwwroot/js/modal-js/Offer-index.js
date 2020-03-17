    (function ($) {
        function Offer() {
            var $this = this;

            function initilizeModel() {
                $("#modal-action-Offer").on('loaded.bs.modal', function (e) {

                }).on('hidden.bs.modal', function (e) {
                    $(this).removeData('bs.modal');
                });
            }
            $this.init = function () {
                initilizeModel();
            }
        }
        $(function () {
            var self = new Offer();
            self.init();
        })
    }(jQuery))