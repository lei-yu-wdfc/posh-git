require 'fileutils'
#creates an msbuild file, replaces the property you wanted and outputs it, then it captures the output
#and returns it back to the called.
def read_msbuild_property(property)
	temp_folder = 'bin\temp' #ENV['Temp']
	FileUtils.makedirs temp_folder
	build_file_name = SecureRandom.base64(6).gsub(/[$=+\/]/,65.+(rand(25)).chr)
	build_file_text = '<Project DefaultTargets="Default" ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">	<Target Name="Default"><Message Text="[[$(msbuildproperty)]]"/></Target></Project>'
	build_file_text = build_file_text.gsub(/msbuildproperty/, property)
	build_file_path = File.join(temp_folder, build_file_name)
	
	File.open build_file_path, "w" do |fl|
		fl.puts build_file_text
	end
	process = IO.popen("msbuild #{build_file_path} /nologo /v:normal")
	output = process.gets('150')
	File.delete build_file_path
	output.match(/\[\[(.*)\]\]/)[1]
end