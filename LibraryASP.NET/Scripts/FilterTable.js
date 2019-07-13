{
	var SortOrder = true;
	var clIndexNameobjSort;
	var TempMasCells;
	var TargeTable = document.getElementsByTagName("table")[0];

	var IdTable = TargeTable.id;
    var RowHeaders =TargeTable.firstElementChild.firstElementChild;
	for (var col of RowHeaders.cells){
		col.addEventListener('click', function handler(event) {
			var rows = document.getElementsByTagName("table")[0].getElementsByTagName("tbody")[0].rows;
			var mass = [];
			for (var i = 0; i < rows.length; i++) {
				mass[i] = rows[i];
			}
			var cellIndex = event.target.cellIndex
			if (cellIndex === clIndexNameobjSort) { SortOrder = false; }
			else { clIndexNameobjSort = cellIndex; SortOrder = true; }
				

			if (SortOrder)
				mass = mass.sort(function (a, b) {
					var TRval1 = a.cells[cellIndex].textContent;
					var TRval2 = b.cells[cellIndex].textContent;
					if (TRval1 > TRval2) return 1;
					if (TRval1 < TRval2) return -1;
					if (TRval1 == TRval2) return 0;
				});
			else { mass = mass.reverse(); }

			$('#'+IdTable+' tbody').detach();
			var person;
			$('#'+IdTable).append(
				'<tbody id="'+IdTable+'">'+(function GetElementMas() {
					for (var i = 0; i < mass.length; i++) {
						person += mass[i].outerHTML;
					}
					return person;
				}()) + '</tbody>').show();
		}
		)
		col.onmouseover = (event) => { event.target.style.cssText = "background-color:#f00;cursor:pointer"; }
		col.onmouseout = (event) => { event.target.style.cssText = "background-color:#fff"; }
	}

}