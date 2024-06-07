<script>
    $(document).ready(function() {
        $('#applyBtn').click(function() {
            const filterBy = $('#filterInput').val().toLowerCase();
            const sortBy = $('#sortSelect').val();

            let rows = $('#employeeTable tr').get();

            rows = rows.filter(row => {
                const name = $(row).find('td:eq(2)').text().toLowerCase();
                return name.includes(filterBy);
            });

            if (sortBy === "name") {
                rows.sort((a, b) => {
                    const nameA = $(a).find('td:eq(2)').text().toLowerCase();
                    const nameB = $(b).find('td:eq(2)').text().toLowerCase();
                    return nameA.localeCompare(nameB);
                });
            } else if (sortBy === "salary") {
                rows.sort((a, b) => {
                    const salaryA = parseFloat($(a).find('td:eq(7)').text());
                    const salaryB = parseFloat($(b).find('td:eq(7)').text());
                    return salaryA - salaryB;
                });
            }

            $.each(rows, (index, row) => {
                $('#employeeTable').append(row);
            });
        });
    });
</script>
