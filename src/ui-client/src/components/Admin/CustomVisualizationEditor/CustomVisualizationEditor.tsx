/* Copyright (c) 2020, UW Medicine Research IT, University of Washington
 * Developed by Nic Dobbins and Cliff Spital, CRIO Sean Mooney
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */ 

import React from 'react';
import { Container, Row, Col } from 'reactstrap';
import AdminState from '../../../models/state/AdminState';
import { VegaLite } from 'react-vega'
import './CustomVisualizationEditor.css';

interface Props { 
    data: AdminState;
    dispatch: any;
}

interface State {
}

export class CustomVisualizationEditor extends React.PureComponent<Props,State> {
    private className = 'custom-visualization-editor';
    constructor(props: Props) {
        super(props);
        this.state = {
        }
    }

    public render() {
        const c = this.className;

        const spec = {
            width: 400,
            height: 200,
            "data": {
                "name": "values"
            },
            "params": [
                {
                "name": "highlight",
                "select": {"type": "point", "on": "mouseover"}
                },
                {"name": "select", "select": "point"},
                { "name": "cornerRadius", "value": 0,
                  "bind": {"input": "range", "min": 0, "max": 50, "step": 1} }
            ],
            "mark": {
                "type": "bar",
                "fill": "#4C78A8",
                "stroke": "black",
                "cursor": "pointer",
                "cornerRadius": {"expr": "cornerRadius"}
            },
            "encoding": {
                "x": {"field": "a", "type": "ordinal"},
                "y": {"field": "b", "type": "quantitative"},
                "fillOpacity": {
                "condition": {"param": "select", "value": 1},
                "value": 0.3
                },
                "strokeWidth": {
                "condition": [
                    {
                    "param": "select",
                    "empty": false,
                    "value": 2
                    },
                    {
                    "param": "highlight",
                    "empty": false,
                    "value": 1
                    }
                ],
                "value": 0
                }
            },
            "config": {
                "scale": {
                "bandPaddingInner": 0.2
                }
            }
        } as any;
    
        const data = { "values": [
            {"a": "A", "b": 28}, {"a": "B", "b": 55}, {"a": "C", "b": 43},
            {"a": "D", "b": 91}, {"a": "E", "b": 81}, {"a": "F", "b": 53},
            {"a": "G", "b": 19}, {"a": "H", "b": 87}, {"a": "I", "b": 52}
            ]
        }

        return (
            <div className={c}>
                <VegaLite spec={spec} data={data}/>
            </div>
        );
    }
}