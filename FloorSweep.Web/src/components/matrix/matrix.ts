import type { HubConnection } from "@microsoft/signalr";
import type MatrixInitMessage from "../../models/messages/MatrixInitMessage";
import type MatrixUpdateMessage from "../../models/messages/MatrixUpdateMessage";

function createData(data: number[],isBinary:boolean,width:number,height:number): Uint8ClampedArray {
    const newData =[] as number[];
    
   
    
        for(let y = 0;y<height;y++)
    {
        for(let x = 0;x<width;x++)
        {
            
            const pixelData = data[(y*height)+x];
           
            if(Number.isNaN(pixelData) ||  isBinary)
            {
                newData.push(pixelData === 0?255:0);
                newData.push(pixelData === -1?255:0);
                newData.push(pixelData !==0 && pixelData !==-1 ?255:0);
            }else{
                newData.push(pixelData);
                newData.push(pixelData);
                newData.push(pixelData);
                
            }
            newData.push(255);
            
        } 

    }
    return new Uint8ClampedArray(newData);
}


export function initializeCanvas(init:MatrixInitMessage,connection:HubConnection, canvas:HTMLCanvasElement)
{
   
    canvas.width = init.width;
    canvas.height = init.height;
        const ctx = canvas.getContext("2d");
        const src = ctx.createImageData(init.width,init.height);
        src.data.set( createData(init.data,init.isBinary,init.width,init.height));
    
        ctx.putImageData(src,0,0);
        
        connection.on("OnMatrixUpdate",(message:MatrixUpdateMessage)=>{
            if(message.name === init.name){
            const newData =createData([message.value],init.isBinary,1,1);
            ctx.putImageData(new ImageData(newData,1,1),message.col,message.row)
        }});

    
}

