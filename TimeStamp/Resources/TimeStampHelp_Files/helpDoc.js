jQuery(document).ready(
    function() {
        $("#helpdoc-element-screenshot .helpdoc-screenshot a").attr('rel', 'gallery').fancybox({
            'titlePosition': 'outside',
            'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
                return '<span id="fancybox-title-over">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? ' &nbsp; ' + title : '') + '</span>';
            }
        });
    }
);