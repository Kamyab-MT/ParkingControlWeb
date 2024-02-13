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
        var dateA = new Date($(a).data('date'));
        var dateB = new Date($(b).data('date'));
        return dateB - dateA;
    });
    $('tbody').empty().append(rows);
}

function sortOlderToNewer(rows) {
    rows.sort(function (a, b) {
        var dateA = new Date($(a).data('date'));
        var dateB = new Date($(b).data('date'));
        return dateA - dateB;
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

function filterByLastYear(rows) {
    var currentDate = new Date();
    var lastYearDate = new Date(currentDate.getFullYear() - 1, currentDate.getMonth(), currentDate.getDate());
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate >= lastYearDate;
    }).show();
}

function filterByLastMonth(rows) {
    var currentDate = new Date();
    var lastMonthDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate());
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate >= lastMonthDate;
    }).show();
}

function filterByLastWeek(rows) {
    var currentDate = new Date();
    var lastWeekStartDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() - 7);
    rows.hide().filter(function () {
        var rowDate = new Date($(this).data('date'));
        return rowDate >= lastWeekStartDate;
    }).show();
}

function showAllRows(rows) {
    rows.show();
}
