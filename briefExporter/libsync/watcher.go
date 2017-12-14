package libsync

import (
	"os"
	"io/ioutil"
	"brief/briefExporter/common"
	"log"
)

type Directory struct {
	Path string
	SubDirectories []*Directory
	Files []os.FileInfo
}

func CheckPath(currentDir *Directory) {
	files, err := ioutil.ReadDir(currentDir.Path)
	common.Check(err)

	for _, file := range files {
		switch isDir := file.IsDir(); isDir {
		case false:
			currentDir.Files = append(currentDir.Files, file)
		case true:
			subDir := &Directory{Path: currentDir.Path + "\\" + file.Name()}
			currentDir.SubDirectories = append(currentDir.SubDirectories, subDir)
			CheckPath(subDir)
		}
	}
}

func (dir *Directory) PrintStructure(initialWhitespace *string)  {
	if initialWhitespace == nil {
		initialWhitespace = new(string)
		*initialWhitespace = ""
	}

	for _, currentDir := range dir.SubDirectories {
		newWhitespace := *initialWhitespace + "\t\t"
		log.Println(*initialWhitespace, currentDir.Path)
		currentDir.PrintStructure(&newWhitespace)
	}

	for _, file := range dir.Files {
		log.Println(*initialWhitespace, file.Name())
	}
}