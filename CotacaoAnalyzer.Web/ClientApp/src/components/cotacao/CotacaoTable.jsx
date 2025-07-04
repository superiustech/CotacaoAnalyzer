import React, { useMemo, useState, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import '../../styles/ag-custom.css';

// IMPORTAÇÃO DO MODULE REGISTRY E MASTERDETAIL
import { ModuleRegistry } from 'ag-grid-community';
import { MasterDetailModule } from 'ag-grid-enterprise';
import { useNavigate } from 'react-router-dom';

// REGISTRAR O MÓDULO
ModuleRegistry.registerModules([MasterDetailModule]);

const CotacaoTable = ({ cotacoes, loading, setSelected }) => {
    const [gridApi, setGridApi] = useState(null);

    // COLUNAS DO GRID PRINCIPAL
    const mainColumns = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox", width: 20, pinned: 'left', suppressMenu: true, suppressSorting: true, suppressFilter: true },
        { headerName: "#", field: "codigoCotacao", cellRenderer: "agGroupCellRenderer", width: 10 },
        { headerName: "Descrição", field: "descricao", width: 200 },
        { headerName: "Data", field: "data", width: 150 },
        { headerName: "Frete incluso", field: "freteIncluso", width: 150 }
    ], []);

    // COLUNAS DO SUBGRID
    const detailColumns = useMemo(() => [
        { headerName: "#", field: "codigoCotacaoItem", width: 10 },
        { headerName: "Sequencial", field: "sequencial", width: 120 },
        { headerName: "Prazo", field: "prazoEntrega", width: 150 },
        { headerName: "Produto", field: "produto.nomeProduto", width: 200 },
        { headerName: "V. Proposto", field: "valorProposto", width: 150 }
    ], []);

    // CONFIGURAÇÃO DO SUBGRID
    const detailCellRendererParams = useMemo(() => ({
        detailGridOptions: {
            columnDefs: detailColumns,
            defaultColDef: {
                flex: 1,
                sortable: true,
                filter: true,
                resizable: true
            },
            animateRows: true
        },
        getDetailRowData: (params) => {
            params.successCallback(params.data.itens || []);
        }
    }), [detailColumns]);

    useEffect(() => {
        if (gridApi && cotacoes.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, cotacoes]);

    return (
        <div>
            <div className="ag-theme-alpine custom" style={{ height: 600, width: '100%' }}>
                <AgGridReact
                    rowData={cotacoes}
                    columnDefs={mainColumns}
                    defaultColDef={{
                        flex: 1,
                        sortable: true,
                        filter: true,
                        resizable: true
                    }}
                    masterDetail={true}
                    detailCellRendererParams={detailCellRendererParams}
                    rowSelection="multiple"
                    onGridReady={(params) => setGridApi(params.api)}
                    onSelectionChanged={(event) => {
                        const selectedNodes = event.api.getSelectedNodes();
                        const selectedData = selectedNodes.map(node => node.data);
                        setSelected(selectedData);
                    }}
                    animateRows={true}
                />
            </div>
        </div>
    );
};

export default CotacaoTable;
