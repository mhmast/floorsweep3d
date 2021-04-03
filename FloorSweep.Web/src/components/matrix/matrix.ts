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
            const pixelValue = isBinary?binaryColors[pixelData]:pixelData;
            newData.push(255);
            newData.push(pixelValue);
            newData.push(pixelValue);
            newData.push(pixelValue);
        } 

    }
    return new Uint8ClampedArray(newData);
}


export function draw(width:number,height:number,data:number[],isBinary:boolean,canvas:HTMLCanvasElement)
{
    const src = createImageBitmap(new ImageData(createData(data,isBinary,width,height),width,height));
    Promise.all([src])
    .then(bmp=>{    
        const ctx = canvas.getContext("2d");
        console.log(bmp[0])
        ctx.drawImage(bmp[0],0,0);
    });
    
}

