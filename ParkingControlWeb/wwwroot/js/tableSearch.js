// tableSearch.js
$(document).ready(function () {
    window.tableSearch = function (dataTable, searchInput) {
        function performSearch(query) {
            var table = $('#' + dataTable);
            table.find('tbody tr').each(function (index, row) {
                var allCells = $(row).find('td').not('.exclude-search');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var escapedQuery = query.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'); // Escape special characters
                        var regExp = new RegExp('\\b' + escapedQuery + '\\b', 'ig'); // Match whole words only
                        var cellContent = $(td).text();
                        var highlightedContent = cellContent.replace(regExp, function (match) {
                            return '<span class="highlight">' + match + '</span>';
                        });
                        $(td).html(highlightedContent);
                        if (regExp.test(cellContent)) {
                            found = true;
                        }
                    });
                    if (found === true) {
                        $(row).show();
                    } else {
                        $(row).hide();
                    }
                }
            });
        }

        $('#' + searchInput).on('input', function () {
            var query = $(this).val();
            performSearch(query);
        });

        // Reset the table when the search input is cleared
        $('#' + searchInput).on('keyup', function (event) {
            if (event.keyCode === 27 || $(this).val().trim() === '') { // Check for Escape key or empty input
                $('#' + dataTable + ' tbody tr').show(); // Show all rows
            }
        });
    };
});
