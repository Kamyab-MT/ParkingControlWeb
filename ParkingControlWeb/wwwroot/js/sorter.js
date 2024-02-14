$(document).ready(function () {
    // Sort rows from newer to older on page load
    var rows = $('tbody tr');
    sortNewerToOlder(rows);

    $('#dateOrderSelect').on('change', function () {
        var selectedSort = $(this).val();
        var rows = $('tbody tr');
        if (selectedSort === 'newerToOlder') {
            sortNewerToOlder(rows);
        } else if (selectedSort === 'olderToNewer') {
            sortOlderToNewer(rows);
        }
    });

    $('#timePeriodSelect').on('change', function () {
        var selectedPeriod = $(this).val();
        var rows = $('tbody tr');
        if (selectedPeriod === 'today') {
            filterByToday(rows);
        } else if (selectedPeriod === 'yesterday') {
            filterByYesterday(rows);
        } else if (selectedPeriod === 'last100') {
            filterLast100(rows);
        } else if (selectedPeriod === 'lastWeek') {
            filterByLastWeek(rows);
        } else if (selectedPeriod === 'lastMonth') {
            filterByLastMonth(rows);
        } else if (selectedPeriod === 'lastYear') {
            filterByLastYear(rows);
        } else if (selectedPeriod === 'all') {
            showAllRows(rows);
        }
    });
});

function sortNewerToOlder(rows) {
    rows.sort(function (a, b) {
        var datetimeA = $(a).data('date').split('T');
        var datetimeB = $(b).data('date').split('T');
        var dateA = new Date(datetimeA[0]);
        var timeA = new Date('1970-01-01T' + datetimeA[1]);
        var dateB = new Date(datetimeB[0]);
        var timeB = new Date('1970-01-01T' + datetimeB[1]);

        return dateB - dateA || timeB - timeA; // If dates are equal, compare times
    });
    $('tbody').empty().append(rows);
}

function sortOlderToNewer(rows) {
    rows.sort(function (a, b) {
        var datetimeA = $(a).data('date').split('T');
        var datetimeB = $(b).data('date').split('T');
        var dateA = new Date(datetimeA[0]);
        var timeA = new Date('1970-01-01T' + datetimeA[1]);
        var dateB = new Date(datetimeB[0]);
        var timeB = new Date('1970-01-01T' + datetimeB[1]);

        return dateA - dateB || timeA - timeB; // If dates are equal, compare times
    });
    $('tbody').empty().append(rows);
}

function filterByToday(rows) {
    var today = new Date().toLocaleDateString();
    rows.hide().filter('[data-date="' + today + '"]').show();
}

function filterByYesterday(rows) {
    var yesterday = new Date(new Date() - 86400000).toLocaleDateString();
    rows.hide().filter('[data-date="' + yesterday + '"]').show();
}

function filterLast100(rows) {
    rows.show().slice(100).hide();
}

function filterByLastWeek(rows) {
    var today = new Date();
    var weekStartDate = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 6);
    var weekEndDate = new Date();
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date').split('T')[0]);
        return rowDate >= weekStartDate && rowDate <= weekEndDate;
    }).show();
}

function filterByLastMonth(rows) {
    var today = new Date();
    var monthStartDate = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());
    var monthEndDate = new Date();
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date').split('T')[0]);
        return rowDate >= monthStartDate && rowDate <= monthEndDate;
    }).show();
}

function filterByLastYear(rows) {
    var today = new Date();
    var yearStartDate = new Date(today.getFullYear() - 1, today.getMonth(), today.getDate());
    var yearEndDate = new Date();
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date').split('T')[0]);
        return rowDate >= yearStartDate && rowDate <= yearEndDate;
    }).show();
}

function showAllRows(rows) {
    rows.show();
}
