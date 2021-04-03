import type { HubConnection } from "@microsoft/signalr";
import type MatrixInitMessage from "../../models/messages/MatrixInitMessage";
import type MatrixUpdateMessage from "../../models/messages/MatrixUpdateMessage";

function createData(data: number[],isBinary:boolean,width:number,height:number): Uint8ClampedArray {
    const newData =[] as number[];
    
    const binaryColors={
        '-1':0,
        '0':128,
        '1':255
    };
        for(let y = 0;y<height;y++)
    {
        for(let x = 0;x<width;x++)
        {
            const pixelData = data[(y*x)+x];
            const pixelValue = isNaN(pixelData) ||  isBinary?binaryColors[pixelData]:pixelData;
            
            newData.push(pixelValue);
            newData.push(pixelValue);
            newData.push(pixelValue);
            newData.push(255);
        } 

    }
    return new Uint8ClampedArray(newData);
}


export function initializeCanvas(init:MatrixInitMessage,connection:HubConnection, canvas:HTMLCanvasElement)
{
    const src = createImageBitmap(new ImageData(createData(init.data,init.isBinary,init.width,init.height),init.width,init.height));
    Promise.all([src])
    .then(bmp=>{    
        const ctx = canvas.getContext("2d");
        ctx.drawImage(bmp[0],0,0);
        connection.on("OnMatrixUpdate",(message:MatrixUpdateMessage)=>{
            if(message.name === init.name){
            const newData =createData([message.value],init.isBinary,1,1);
            ctx.putImageData(new ImageData(newData,1,1),message.col,message.row)
        }});
    
    });
    
}

