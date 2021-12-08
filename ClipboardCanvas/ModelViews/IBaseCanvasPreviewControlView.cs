﻿using ClipboardCanvas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardCanvas.ModelViews
{
    public interface IBaseCanvasPreviewControlView
    {
        ICollectionModel CollectionModel { get; }
    }
}
